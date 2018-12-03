using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model.OrderReport
{
    public class TransportOperationBlock
    {
        public int Id { get; set; }

        public int FirstWorkshopId { get; set; }
        public string FirstWorkshopName { get; set; }

        public int? SecondWorkshopId { get; set; }
        public string SecondWorkshopName { get; set; }

        public float Distance { get; set; }

        public long Duration { get; set; }

        public int GroupBlockId { get; set; }
        public virtual GroupBlock GroupBlock { get; set; }
    }
}
