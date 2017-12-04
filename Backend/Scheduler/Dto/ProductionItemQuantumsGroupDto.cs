using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto
{
    public class ProductionItemQuantumsGroupDto
    {
        public List<ProductionItemQuantumDto> ProductionItemQuantums { get; set; }

        public List<int> WorkshopSequence { get; set; }

        public List<long> WorkshopStartTimes { get; set; } = new List<long>();
        public List<long> WorkshopEndTimes { get; set; } = new List<long>();
        public List<long> WorkshopDurations { get; set; } = new List<long>();
    }
}
