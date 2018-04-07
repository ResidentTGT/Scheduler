using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto
{
    public class OperationDto
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public long MainTime { get; set; }

        public long AdditionalTime { get; set; }

        public OperationType Type { get; set; }

        public EquipmentDto Equipment { get; set; }

        public DetailDto Detail { get; set; }

        public int DetailId { get; set; }

        public int RiggingCost { get; set; }

        public int RiggingStorageCost { get; set; }
    }
}
