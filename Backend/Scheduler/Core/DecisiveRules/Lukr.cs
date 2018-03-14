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

        internal static void SortGroups(ProductionItem productionItem)
        {
            productionItem.ProductionItemQuantumsGroups = productionItem.ProductionItemQuantumsGroups.OrderBy(r => SumWorkshopDurations(r.WorkshopDurations)).ToList();
        }

        private static TimeSpan SumWorkshopDurations(List<TimeSpan> workshopDurations)
        {
            TimeSpan sum = new TimeSpan();
            foreach (var time in workshopDurations)
                sum += time;
            return sum;
        }

        private static TimeSpan SumDurations(Detail detail)
        {
            TimeSpan sum = new TimeSpan();
            foreach (var operation in detail.Routes.First().Operations.Where(o => o.Equipment.Workshop != null))
                sum += operation.MainTime;
            return sum;
        }
    }
}
