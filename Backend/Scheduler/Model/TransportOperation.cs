using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public class TransportOperation
    {
        public int Id { get; set; }

        public int FirstWorkshopId { get; set; }
        public virtual Workshop FirstWorkshop { get; set; }

        public int? SecondWorkshopId { get; set; }
        public virtual Workshop SecondWorkshop { get; set; }

        /// <summary>
        /// Расстояние между цехами
        /// </summary>
        public float Distance { get; set; }

        public TimeSpan Duration { get; set; }

        public int TransportId { get; set; }
        public virtual Transport Transport { get; set; }
    }
}
