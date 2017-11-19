﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto
{
    public class OrderQuantumDto
    {
        public int Id { get; set; }

        public int ProductionItemId { get; set; }

        public string ProductionItemTitle { get; set; }

        public int OrderId { get; set; }

        public int Count { get; set; }

        public int ItemsCountInOnePart { get; set; }
    }
}