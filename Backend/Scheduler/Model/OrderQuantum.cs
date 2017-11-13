using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    /// <summary>
    /// Представляет собой квант заказа, т.е. тип производимого изделия и его кол-во
    /// </summary>
    public class OrderQuantum
    {
        public int Id { get; set; }

        public ProductionItem ProductionItem { get; set; }
        public int ProductionItemId { get; set; }

        public Order Order { get; set; }
        public int OrderId { get; set; }

        public int Count { get; set; }
    }
}
