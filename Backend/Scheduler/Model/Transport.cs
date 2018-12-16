using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public class Transport
    {
        public int Id { get; set; }

        /// <summary>
        /// кг
        /// </summary>
        public int LoadCapacity { get; set; }

        /// <summary>
        /// Время загрузки/выгрузки
        /// </summary>
        public TimeSpan UnloadingTime { get; set; }

        /// <summary>
        /// Средняя скорость, км/ч
        /// </summary>
        public float AverageSpeed { get; set; }

        [NotMapped]
        public bool? IsFree { get; set; }

        public bool IsAvailable { get; set; } = true;

        public virtual ICollection<TransportOperation> TransportOperations { get; set; }
    }
}
