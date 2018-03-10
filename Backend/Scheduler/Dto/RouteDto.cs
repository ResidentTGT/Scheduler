using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto
{
    public class RouteDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MachiningOperationsCount { get; set; }

        public int TransportOperationsCount { get; set; }

        public int AssemblingOperationsCount { get; set; }

        public List<OperationDto> Operations { get; set; }

        public DetailDto Detail { get; set; }

        public List<int> OperationsSequence { get; set; }
    }
}
