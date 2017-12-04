using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto
{
    public class OrderQuantumDto
    {
        public int? Id { get; set; }

        public ProductionItemDto ProductionItem { get; set; }

        public int OrderId { get; set; }

        public int Count { get; set; }

        public int ItemsCountInOnePart { get; set; }

        public long? MachiningFullPartTime { get; set; }
        public long? MachiningRemainingFromPartsTime { get; set; }

        public long? AssemblingFullBatchTime { get; set; }
        public long? AssemblingFullPartTime { get; set; }
        public long? AssemblingRemainingFromPartsTime { get; set; }

        public List<long> MachiningStartTimes { get; set; }
        public List<long> MachiningEndTimes { get; set; }
        public List<long> MachiningDurations { get; set; }

        public List<long> AssemblingStartTimes { get; set; }
        public List<long> AssemblingEndTimes { get; set; }
        public List<long> AssemblingDurations { get; set; }
    }
}
