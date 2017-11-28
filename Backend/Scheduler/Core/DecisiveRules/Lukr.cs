using Scheduler.Core.Grouping;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.DecisiveRules
{
    public static class Lukr
    {
        internal static void SortDetails(ProductionItemQuantumsGroup productionItemQuantumsGroup)
        {
            productionItemQuantumsGroup.ProductionItemQuantums = productionItemQuantumsGroup.ProductionItemQuantums.OrderBy(r => SumDurations(r.Detail)).ToList();
        }

        private static TimeSpan SumDurations(Detail detail)
        {
            TimeSpan sum = new TimeSpan();
            foreach (var operation in detail.Operations)
                sum += operation.MainTime;
            return sum;
        }
    }
}
