using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto.Reporting
{
    public class DetailsBatchBlockDto
    {
        public int DetailId { get; set; }

        public string DetailName { get; set; }

        public int DetailsCount { get; set; }

        public long StartTime { get; set; }

        public long Duration { get; set; }
    }
}
