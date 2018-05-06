using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto.Reporting
{
    class OrderReportDto
    {
        public int Id { get; set; }

        public DateTime CreationTime { get; set; }

        public OrderDto Order { get; set; }

        public List<OrderBlockDto> OrderBlocks { get; set; }
    }
}
