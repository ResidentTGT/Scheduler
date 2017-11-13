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
    }
}
