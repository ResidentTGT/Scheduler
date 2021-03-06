﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public class Conveyor
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TimeSpan TransportTime { get; set; }

        public TimeSpan ReadjustingTime { get; set; }

        public virtual ICollection<Equipment> Equipments { get; set; }
    }
}
