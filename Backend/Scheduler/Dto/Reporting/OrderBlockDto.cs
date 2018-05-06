using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto.Reporting
{
    public class OrderBlockDto
    {
        public int ProductionItemId { get; set; }

        public string ProductionItemsName { get; set; }

        public int ProductionItemsCount { get; set; }

        public long StartTime { get; set; }

        public long Duration { get; set; }

        public bool IsMachining { get; set; }

        public List<GroupBlockDto> GroupBlocks { get; set; }
    }
}
