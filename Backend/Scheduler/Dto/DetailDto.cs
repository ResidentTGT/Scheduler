using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto
{
    public class DetailDto
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? Cost { get; set; }

        public bool? IsPurchased { get; set; }

        public string RouteName { get; set; }

        public List<int> WorkshopSequence { get; set; }
    }
}
