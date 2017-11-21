using Scheduler.Database;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Assembling
{

    public class AssemblingTime
    {
        private DbManager _dbManager;

        public AssemblingTime(DbManager dbManager)
        {
            _dbManager = dbManager;
        }

        public List<OrderQuantumAssemblingTime> CalculateAssemblingTime(Order order)
        {
            foreach (var orderQuantum in order.OrderQuantums)
            {
                var operations = _dbManager.GetOperationsByProductionItemId(orderQuantum.ProductionItem.Id).ToList();
            }
            throw new NotImplementedException();
        }
    }
}
