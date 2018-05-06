using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    /// <summary>
    /// Представляет собой квант изделия\узла, т.е. тип входящей детали и её кол-во
    /// </summary>
    public class ProductionItemQuantum
    {
        public int Id { get; set; }

        public Detail Detail { get; set; }
        public int DetailId { get; set; }

        public ProductionItem ProductionItem { get; set; }
        public int ProductionItemId { get; set; }

        public int Count { get; set; }

        /// <summary>
        /// Все времена располагаются в порядке прохождения деталью станков
        /// </summary>
        public List<TimeSpan> StartTimes { get; set; } = new List<TimeSpan>();
        public List<TimeSpan> EndTimes { get; set; } = new List<TimeSpan>();
        public List<TimeSpan> MachiningDurations { get; set; } = new List<TimeSpan>();
    }
}
