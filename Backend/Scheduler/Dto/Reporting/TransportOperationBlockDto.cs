using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto.Reporting
{
    public class TransportOperationBlockDto
    {
        public int Id { get; set; }

        public int FirstWorkshopId { get; set; }
        public string FirstWorkshopName { get; set; }

        public int? SecondWorkshopId { get; set; }
        public string SecondWorkshopName { get; set; }

        public float Distance { get; set; }

        public long Duration { get; set; }
    }
}
