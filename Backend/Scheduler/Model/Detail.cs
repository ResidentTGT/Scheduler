using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public class Detail
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? Cost { get; set; }

        public bool? IsPurchased { get; set; }

        public virtual List<ProductionItem> ProductionItems { get; set; }

        public Route Route { get; set; }
        public int? RouteId { get; set; }
    }
}
