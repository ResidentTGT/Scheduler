using Scheduler.Dto;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Scheduler.Database
{
    public class DbManager
    {
        private SchedulerContext _context;

        public DbManager()
        {
            _context = new SchedulerContext();
        }

        #region Details
        public IEnumerable<Detail> GetDetails()
        {
            var details = _context.Details.Include("Route").Include("ProductionItems").Include("ProductionItemQuantums").Include("Operations").ToList();
            return details as IEnumerable<Detail>;
        }

        public IEnumerable<Detail> GetDetailsWithoutRoutes()
        {
            var details = _context.Details.Where(d => d.Route == null).Include("Route").Include("ProductionItems").Include("ProductionItemQuantums").Include("Operations");
            return details as IEnumerable<Detail>;
        }

        public int CreateDetail(Detail detail)
        {
            _context.Details.Add(detail);
            _context.SaveChanges();

            return detail.Id;
        }

        public void DeleteDetail(int id)
        {
            var detail = _context.Details.First(d => d.Id == id);

            if (detail.Route != null)
                _context.Routes.Remove(detail.Route);
            _context.Details.Remove(detail);

            _context.SaveChanges();
        }

        public Detail GetDetailById(int? id)
        {
            var detail = _context.Details.First(pi => pi.Id == id);
            return detail;
        }
        #endregion

        #region EquipmentMethods
        public IEnumerable<Equipment> GetEquipments()
        {
            var equipments = _context.Equipments.Include("Operations").Include("Conveyor").Include("Workshop");
            return equipments as IEnumerable<Equipment>;
        }

        public int CreateEquipment(Equipment equipment)
        {
            _context.Equipments.Add(equipment);
            _context.SaveChanges();

            return equipment.Id;
        }

        public void DeleteEquipment(int id)
        {
            _context.Equipments.Remove(_context.Equipments.First(d => d.Id == id));
            _context.SaveChanges();
        }
        public Equipment GetEquipmentById(int? id)
        {
            var equipment = _context.Equipments.First(pi => pi.Id == id);
            return equipment;
        }
        public Equipment GetEquipmentByOperationId(int id)
        {
            var equipment = _context.Operations.Include(o => o.Equipment.Conveyor).First(o => o.Id == id).Equipment;
            return equipment;
        }
        #endregion

        #region Orders
        public IEnumerable<Order> GetOrders()
        {
            var orders = _context.Orders
                .Include(o => o.OrderQuantums.Select(op => op.ProductionItem))
                .Include(o => o.OrderQuantums.Select(op => op.ProductionItem.ProductionItemQuantums))
                .Include(o => o.OrderQuantums.Select(op => op.ProductionItem.ProductionItemQuantums.Select(pi => pi.Detail)))
                .Include(o => o.OrderQuantums.Select(op => op.ProductionItem.ProductionItemQuantums.Select(pi => pi.Detail).Select(d => d.Operations)))
                .Include(o => o.OrderQuantums.Select(op => op.ProductionItem.ProductionItemQuantums.Select(pi => pi.Detail).Select(d => d.Operations.Select(oper => oper.Equipment))));

            return orders as IEnumerable<Order>;
        }

        public Order GetOrderById(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderQuantums.Select(oq => oq.ProductionItem))
                .Include(o => o.OrderQuantums.Select(op => op.ProductionItem.ProductionItemQuantums))
                .Include(o => o.OrderQuantums.Select(op => op.ProductionItem.ProductionItemQuantums.Select(pi => pi.Detail)))
                .Include(o => o.OrderQuantums.Select(op => op.ProductionItem.ProductionItemQuantums.Select(pi => pi.Detail).Select(d => d.Operations)))
                .Include(o => o.OrderQuantums.Select(op => op.ProductionItem.ProductionItemQuantums.Select(pi => pi.Detail).Select(d => d.Operations.Select(oper => oper.Equipment))))
                .Include(o => o.OrderQuantums.Select(op => op.ProductionItem.ProductionItemQuantums.Select(pi => pi.Detail).Select(d => d.Operations.Select(oper => oper.Equipment.Workshop))))
                 .Include(o => o.OrderQuantums.Select(op => op.ProductionItem.ProductionItemQuantums.Select(pi => pi.Detail).Select(d => d.Operations.Select(oper => oper.Equipment.Conveyor))))
                .First(o => o.Id == id);
            return order;
        }

        public int CreateOrder(Order order)
        {
            var orderQuantums = new List<OrderQuantum>();
            foreach (var orderQuantum in order.OrderQuantums)
                _context.OrderQuantums.Add(new OrderQuantum()
                {
                    Count = orderQuantum.Count,
                    ItemsCountInOnePart = orderQuantum.ItemsCountInOnePart,
                    ProductionItemId = orderQuantum.ProductionItemId
                });
            order.OrderQuantums = orderQuantums;

            _context.Orders.Add(order);
            _context.SaveChanges();

            return order.Id;
        }

        public void DeleteOrder(int id)
        {
            _context.Orders.Remove(_context.Orders.First(d => d.Id == id));
            _context.SaveChanges();
        }

        public void SetOrderState(int id, OrderState state)
        {
            var order = _context.Orders.First(o => o.Id == id);
            order.State = state;
            _context.SaveChanges();
        }
        #endregion

        #region ProductionItems
        public IEnumerable<ProductionItem> GetProductionItems()
        {
            var productionItems = _context.ProductionItems
                .Include(p => p.ProductionItemQuantums.Select(po => po.Detail)
                .Select(d => d.Operations.Select(o => o.Equipment).Select(e => e.Workshop)))
                .Include("OrderQuantums");
            return productionItems as IEnumerable<ProductionItem>;
        }

        public ProductionItem GetProductionItemById(int? id)
        {
            var productionItem = _context.ProductionItems.First(pi => pi.Id == id);
            return productionItem;
        }

        public int CreateProductionItem(ProductionItem productionItem)
        {
            var productionItemQuantums = new List<ProductionItemQuantum>();

            foreach (var productionItemQuantum in productionItem.ProductionItemQuantums)
                _context.ProductionItemQuantums.Add(new ProductionItemQuantum()
                {
                    Count = productionItemQuantum.Count,
                    DetailId = productionItemQuantum.Detail.Id,
                });
            productionItem.ProductionItemQuantums = productionItemQuantums;

            _context.ProductionItems.Add(productionItem);
            _context.SaveChanges();

            return productionItem.Id;
        }

        public void DeleteProductionItem(int id)
        {
            _context.ProductionItems.Remove(_context.ProductionItems.First(d => d.Id == id));
            _context.ProductionItems.Where(pi => pi.ParentProductionItemId == id).ToList().ForEach(p => p.ParentProductionItemId = null);
            _context.SaveChanges();
        }
        #endregion

        #region Operations
        public IEnumerable<Operation> GetOperations()
        {
            var operations = _context.Operations
                .Include(o => o.Detail)
                .Include(o => o.Equipment)
                .Include(o => o.Equipment.Conveyor)
                .Include(o => o.Equipment.Workshop)
                .Include(o => o.Routes);
            return operations as IEnumerable<Operation>;
        }

        public IEnumerable<Operation> GetOperationsByDetailId(int detailId)
        {
            var operations = _context.Operations.Where(o => o.DetailId == detailId)
                .Include(o => o.Detail)
                .Include(o => o.Equipment)
                .Include(o => o.Equipment.Conveyor)
                .Include(o => o.Equipment.Workshop)
                .Include(o => o.Routes);
            return operations as IEnumerable<Operation>;
        }

        public Operation GetOperationById(int id)
        {
            var operation = _context.Operations.First(o => o.Id == id);
            return operation;
        }

        public IEnumerable<Operation> GetOperationsByProductionItemId(int id)
        {
            var operations = _context.ProductionItems
                .Include(pi => pi.ProductionItemQuantums.Select(piq => piq.Detail))
                .First(p => p.Id == id)
                .ProductionItemQuantums
                .Select(piq => piq.Detail.Route)
                .SelectMany(d => d.Operations);

            return operations;
        }

        public int CreateOperation(Operation operation)
        {
            _context.Operations.Add(operation);
            _context.SaveChanges();

            return operation.Id;
        }

        public void DeleteOperation(int id)
        {
            _context.Operations.Remove(_context.Operations.First(d => d.Id == id));
            _context.SaveChanges();
        }
        #endregion

        #region Routes
        public IEnumerable<Route> GetRoutes()
        {
            var routes = _context.Routes
                .Include("Detail")
                .Include(r => r.Operations.Select(o => o.Equipment.Workshop))
                .Include(r => r.Operations.Select(o => o.Equipment.Conveyor))
                .Include(r => r.Operations.Select(o => o.Equipment));

            return routes as IEnumerable<Route>;
        }

        public Route GetRouteById(int id)
        {
            var route = _context.Routes.First(r => r.Id == id);
            return route;
        }

        public int CreateRoute(Route route)
        {
            var operIds = route.Operations.ToList().Select(o => o.Id).ToList();
            route.Operations = new List<Operation>();
            _context.Routes.Add(route);

            foreach (var operId in operIds)
            {
                route.Operations.Add(GetOperationById(operId));
            }

            _context.SaveChanges();

            return route.Id;
        }



        public void DeleteRoute(int id)
        {
            _context.Routes.Remove(_context.Routes.First(d => d.Id == id));
            _context.SaveChanges();
        }
        #endregion


        #region ConveyorsAndWorkshops
        public IEnumerable<Conveyor> GetConveyors()
        {
            var conveyors = _context.Conveyors;
            return conveyors as IEnumerable<Conveyor>;
        }

        public Conveyor GetConveyorById(int id)
        {
            var conveyor = _context.Conveyors.First(c => c.Id == id);
            return conveyor;
        }

        public IEnumerable<Workshop> GetWorkshops()
        {
            var workshops = _context.Workshops;
            return workshops as IEnumerable<Workshop>;
        }

        public Conveyor GetConveyorByEquipmentId(int id)
        {
            var conveyor = _context.Equipments.First(c => c.Id == id).Conveyor;
            return conveyor;
        }
        #endregion
    }
}
