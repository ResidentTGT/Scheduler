﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model.OrderReport
{
    public class OrderBlock
    {
        public int Id { get; set; }

        public int OrderReportId { get; set; }
        public virtual OrderReport OrderReport { get; set; }

        public int ProductionItemId { get; set; }

        public int ProductionItemsCount { get; set; }

        public string ProductionItemsName { get; set; }

        public long StartTime { get; set; }

        public long Duration { get; set; }

        public bool IsMachining { get; set; }

        public virtual List<GroupBlock> GroupBlocks { get; set; }
    }
}
