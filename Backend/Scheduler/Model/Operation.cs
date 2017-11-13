using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public enum OperationType
    {
        Undefined = 0,

        Machining = 1,

        Assembling = 2,

        Transport = 3
    }

    public class Operation
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public TimeSpan MainTime { get; set; }

        /// <summary>
        /// Если операция обработки, то доп. время - время переналадки станка перед обработкой
        /// </summary>
        public TimeSpan AdditionalTime { get; set; }

        public OperationType Type { get; set; }

        public Equipment Equipment { get; set; }
        public int? EquipmentId { get; set; }

        public virtual List<Route> Routes { get; set; }
    }
}
