using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.Assembling
{
    public class OrderQuantumAssemblingTime
    {
        public ProductionItem ProductionItem { get; set; }

        public TimeSpan FullBatchTime { get; set; }

        public TimeSpan ProductionsItemsPartTime { get; set; }

        public TimeSpan? RemainingFromPartsTime { get; set; }
    }
}
