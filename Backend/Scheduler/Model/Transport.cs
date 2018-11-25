using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public class Transport
    {
        public int Id { get; set; }

        /// <summary>
        /// см
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// см
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// см
        /// </summary>
        public int Length { get; set; }

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

        public bool? IsFree { get; set; }
    }
}
