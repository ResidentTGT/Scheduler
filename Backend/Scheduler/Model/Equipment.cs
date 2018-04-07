using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public enum EquipmentType
    {
        Undefined = 0,

        MachiningTool = 1,

        AssemblyWorkplace = 2,

        Transport = 3
    }

    public class Equipment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public EquipmentType Type { get; set; }

        /// <summary>
        /// Стоимость оборудования
        /// </summary>
        public int Cost { get; set; }

        /// <summary>
        /// Ресурс времени использования оборудования в течение исследуемого интервала времени
        /// </summary>
        public double UsingTimeResource { get; set; }

        /// <summary>
        /// Загрузка оборудования
        /// </summary>
        public double LoadFactor { get; set; }

        /// <summary>
        /// Стоимость обслуживания
        /// </summary>
        public int MaintenanceCost { get; set; }

        public int? WorkshopId { get; set; }
        public Workshop Workshop { get; set; }

        public int? ConveyorId { get; set; }
        public Conveyor Conveyor { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }
    }
}
