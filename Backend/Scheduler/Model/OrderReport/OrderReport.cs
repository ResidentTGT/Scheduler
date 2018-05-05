using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model.OrderReport
{
    public class OrderReport
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public DateTime CreationTime { get; set; }

        public virtual List<OrderBlock> OrderBlocks { get; set; }
    }
}
