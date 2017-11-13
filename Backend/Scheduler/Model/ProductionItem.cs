using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public class ProductionItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Если элемент является узлом, то у него есть "родитель" в виде узла или изделия
        /// </summary>
        public int? ParentProductionItemId { get; set; }

        public bool IsNode { get; set; } = false;

        public List<int> DetailsIds { get; set; }

        public List<int> DetailsCounts { get; set; }

        public virtual List<Detail> Details { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
