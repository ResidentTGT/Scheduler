using Scheduler.Log;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Core.Grouping;

namespace Scheduler.Core.CountingTime
{
    internal static class GroupsTiming
    {
        private static int _currentTime { get; set; }

        public static TimeSpan CountProductionItemMachiningTime(OrderQuantum orderQuantum, List<Transport> transports, bool isFinally)
        {
            Logger.Log($"Начат расчет времени части партии изделия: {orderQuantum.ProductionItem.Title}", LogLevel.Info);

            var groups = orderQuantum.ProductionItem.ProductionItemQuantumsGroups;

            var productionItemTime = new TimeSpan(0);

            for (var i = 0; i < groups.Count; i++)
            {
                var currentGroup = groups[i];

                CountTimesForGroup(currentGroup, transports);

                if (i != 0)
                {
                    TimeSpan maxDiff = new TimeSpan(0);

                    for (var k = 0; k < currentGroup.WorkshopSequence.Count; k++)
                    {
                        for (var groupIndex = 1; groupIndex <= i; groupIndex++)
                        {
                            var prevGroup = groups[i - groupIndex];

                            if (prevGroup.WorkshopSequence.Contains(currentGroup.WorkshopSequence[k]))
                            {
                                var diff = prevGroup
                                    .WorkshopEndTimes[prevGroup.WorkshopSequence.IndexOf(currentGroup.WorkshopSequence[k])]
                                    - currentGroup.WorkshopStartTimes[k];
                                if (diff > maxDiff)
                                    maxDiff = diff;
                            }
                        }
                    }

                    for (var f = 0; f < groups[i].WorkshopSequence.Count; f++)
                    {
                        groups[i].WorkshopStartTimes[f] += maxDiff;
                        groups[i].WorkshopEndTimes[f] += maxDiff;
                    }
                }
            }

            long max = 0;
            foreach (var group in groups)
                max = (Math.Max(max, group.WorkshopEndTimes.Max().Ticks));
            productionItemTime = new TimeSpan(max);


            if (isFinally)
            {
                orderQuantum.MachiningFullPartTime = productionItemTime;
            }
            else
            {
                foreach (var group in groups)
                {
                    group.WorkshopStartTimes.Clear();
                    group.WorkshopEndTimes.Clear();
                }
            }

            Logger.Log($"Закончен расчет времени части партии изделия: {orderQuantum.ProductionItem.Title}. Суммарное время: {productionItemTime}.", LogLevel.Info);

            return productionItemTime;
        }

        private static void CountTimesForGroup(ProductionItemQuantumsGroup group, List<Transport> transports)
        {
            for (var j = 0; j < group.WorkshopSequence.Count; j++)
            {
                if (j == 0)
                {
                    group.WorkshopStartTimes.Add(new TimeSpan(0));
                    group.WorkshopEndTimes.Add(group.WorkshopDurations[j]);
                }
                else
                {
                    group.WorkshopStartTimes.Add(group.WorkshopEndTimes[j - 1]);
                    group.WorkshopEndTimes.Add(group.WorkshopStartTimes[j] + group.WorkshopDurations[j]);
                }
            }
        }
    }
}
