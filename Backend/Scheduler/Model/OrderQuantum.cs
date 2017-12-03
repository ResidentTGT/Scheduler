using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        public int ItemsCountInOnePart { get; set; }

        [NotMapped]
        public TimeSpan MachiningFullPartTime { get; set; }
        [NotMapped]
        public TimeSpan MachiningRemainingFromPartsTime { get; set; }

        [NotMapped]
        public TimeSpan AssemblingFullBatchTime { get; set; }
        [NotMapped]
        public TimeSpan AssemblingFullPartTime { get; set; }
        [NotMapped]
        public TimeSpan? AssemblingRemainingFromPartsTime { get; set; }

        public List<TimeSpan> MachiningStartTimes { get; set; }
        public List<TimeSpan> MachiningEndTimes { get; set; }
        public List<TimeSpan> MachiningDurations { get; set; }

        public List<TimeSpan> AssemblingStartTimes { get; set; }
        public List<TimeSpan> AssemblingEndTimes { get; set; }
        public List<TimeSpan> AssemblingDurations { get; set; }
    }
}
