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
            var details = _context.Details;
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
        #endregion

        #region EquipmentMethods
        public IEnumerable<Equipment> GetEquipments()
        {
            var equipments = _context.Equipments;
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
        #endregion

        #region Orders
        public IEnumerable<Order> GetOrders()
        {
            var orders = _context.Orders;
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
    }
}
