using Scheduler.Database;
using Scheduler.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Model;

namespace Scheduler.Core
{
    internal class Calculation
    {
        private DbManager _dbManager;
        private DtoConverter _dtoConverter;

        public Calculation()
        {
            _dbManager = new DbManager();
            _dtoConverter = new DtoConverter();
        }

        internal void CalculateOrder(int orderId)
        {
            var order = _dbManager.GetOrderById(orderId);

          //  var assemblingTimes = CalculateAssemblingTimes(order);

        }


        private void CalculateAssemblingTimes(Order order)
        {
            
        }
    }
}
