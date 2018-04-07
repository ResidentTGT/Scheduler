using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto
{
    public class EquipmentDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public EquipmentType Type { get; set; }

        public WorkshopDto Workshop { get; set; }

        public ConveyorDto Conveyor { get; set; }

        public int Cost { get; set; }

        /// <summary>
        /// в часах
        /// </summary>
        public double UsingTimeResource { get; set; }

        public double LoadFactor { get; set; }

        public int MaintenanceCost { get; set; }
    }
}
