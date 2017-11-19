using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public class Workshop
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Equipment> Equipments { get; set; }
    }
}
