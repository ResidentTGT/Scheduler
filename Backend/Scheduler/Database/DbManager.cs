using Scheduler.Dto;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var details = _context.Details.Include("Route").Include("ProductionItems").Include("ProductionItemQuantums").Include("Operations");
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
            _context.Details.Remove(_context.Details.First(d => d.Id == id));
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
        #endregion

        #region Orders
        public IEnumerable<Order> GetOrders()
        {
            var orders = _context.Orders.Include("OrderQuantums");
            return orders as IEnumerable<Order>;
        }
        public int CreateOrder(Order Order)
        {
            _context.Orders.Add(Order);
            _context.SaveChanges();

            return Order.Id;
        }

        public void DeleteOrder(int id)
        {
            _context.Orders.Remove(_context.Orders.First(d => d.Id == id));
            _context.SaveChanges();
        }
        #endregion

        #region ProductionItems
        public IEnumerable<ProductionItem> GetProductionItems()
        {
            var productionItems = _context.ProductionItems.Include("ProductionItemQuantums").Include("OrderQuantums");
            return productionItems as IEnumerable<ProductionItem>;
        }

        public ProductionItem GetProductionItemById(int? id)
        {
            var productionItem = _context.ProductionItems.First(pi => pi.Id == id);
            return productionItem;
        }

        public int CreateProductionItem(ProductionItem productionItem)
        {
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
            var operations = _context.Operations.Include("Detail").Include("Equipment").Include("Routes");
            return operations as IEnumerable<Operation>;
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

        public IEnumerable<Conveyor> GetConveyors()
        {
            var conveyors = _context.Conveyors;
            return conveyors as IEnumerable<Conveyor>;
        }

        public IEnumerable<Workshop> GetWorkshops()
        {
            var workshops = _context.Workshops;
            return workshops as IEnumerable<Workshop>;
        }
    }
}
