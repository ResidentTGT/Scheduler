using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public OrderState State { get; set; }

        public DateTime? PlannedBeginDate { get; set; }

        public DateTime? PlannedEndDate { get; set; }

        public List<OrderQuantumDto> OrderQuantums { get; set; }

        public int OrderQuantumsCount { get; set; }
    }
}
