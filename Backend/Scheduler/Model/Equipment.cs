using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public enum EquipmentType
    {
        Undefined = 0,

        MachiningTool = 1,

        AssemblyWorkplace = 2,

        Transport = 3
    }

    public class Equipment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public EquipmentType Type { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }
    }
}
