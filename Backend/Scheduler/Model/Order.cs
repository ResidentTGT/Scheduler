using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public enum OrderState
    {
        Undefined = 0,

        Ready = 1,

        InProcess = 2
    }

    public class Order
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public OrderState State { get; set; }

        public DateTime? PlannedBeginDate { get; set; }

        public DateTime? PlannedEndDate { get; set; }

        public virtual List<OrderQuantum> OrderQuantums { get; set; }
    }
}
