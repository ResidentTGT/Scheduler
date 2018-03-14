using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public class Route
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string OperationsSequence { get; set; }

        public virtual List<Operation> Operations { get; set; }

        public virtual Detail Detail { get; set; }
        public int? DetailId { get; set; }

    }
}
