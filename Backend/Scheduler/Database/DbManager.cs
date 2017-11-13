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

        public IEnumerable<Detail> GetDetails()
        {
            var details = _context.Details;
            return details;
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

        public IEnumerable<Equipment> GetEquipments()
        {
            var equipments = _context.Equipments;
            return equipments;
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
    }
}
