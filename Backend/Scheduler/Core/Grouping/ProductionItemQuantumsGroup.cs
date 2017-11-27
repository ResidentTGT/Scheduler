using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.Grouping
{
    public class ProductionItemQuantumsGroup
    {
        public List<ProductionItemQuantum> ProductionItemQuantums { get; set; }

        public List<int> WorkshopSequence { get; set; }
    }
}
