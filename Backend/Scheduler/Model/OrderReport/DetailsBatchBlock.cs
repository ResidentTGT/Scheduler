using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model.OrderReport
{
    public class DetailsBatchBlock
    {
        public int Id { get; set; }

        public int DetailId { get; set; }

        public string DetailName { get; set; }

        public int DetailsCount { get; set; }

        public int GroupBlockId { get; set; }
        public virtual GroupBlock GroupBlock { get; set; }

        public long StartTime { get; set; }

        public long Duration { get; set; }

        public virtual Equipment Equipment { get; set; }
        public int EquipmentId { get; set; }
    }
}
