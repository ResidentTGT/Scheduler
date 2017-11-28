using Scheduler.Core.Grouping;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.DecisiveRules
{
    internal static class Spt
    {
        internal static void SortDetails(ProductionItemQuantumsGroup productionItemQuantumsGroup)
        {
            productionItemQuantumsGroup.ProductionItemQuantums = productionItemQuantumsGroup.ProductionItemQuantums.OrderBy(r => r.Detail.Operations.First().MainTime).ToList();
        }

    }
}
