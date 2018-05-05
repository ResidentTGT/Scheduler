﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model.OrderReport
{
    public class GroupBlock
    {
        public int Id { get; set; }

        public int OrderBlockId { get; set; }
        public virtual OrderBlock OrderBlock { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public virtual Workshop Workshop { get; set; }
        public int WorkshopId { get; set; }

        public virtual List<DetailsBatchBlock> DetailsBatchBlocks { get; set; }
    }
}
