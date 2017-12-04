using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto
{
    public class ProductionItemQuantumDto
    {
        public int Id { get; set; }

        public DetailDto Detail { get; set; }

        public int ProductionItemId { get; set; }

        public int Count { get; set; }

        public List<long> StartTimes { get; set; }
        public List<long> EndTimes { get; set; }
        public List<long> MachiningDurations { get; set; }
    }
}
