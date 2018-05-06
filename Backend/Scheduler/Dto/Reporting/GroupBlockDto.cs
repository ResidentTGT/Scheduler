using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto.Reporting
{
    public class GroupBlockDto
    {
        public int Id { get; set; }

        public WorkshopDto Workshop { get; set; }

        public int GroupIndex { get; set; }

        public long StartTime { get; set; }

        public long Duration { get; set; }

        public List<DetailsBatchBlockDto> DetailsBatchBlocks { get; set; }
    }
}
