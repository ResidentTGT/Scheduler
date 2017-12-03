using Scheduler.Log;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.CountingTime
{
    internal static class GroupsTiming
    {
        public static TimeSpan CountProductionItemMachiningTime(OrderQuantum orderQuantum, bool isFinally)
        {
            Logger.Log($"Начат расчет времени части партии изделия: {orderQuantum.ProductionItem.Title}", LogLevel.Info);

            var groups = orderQuantum.ProductionItem.ProductionItemQuantumsGroups;

            var productionItemTime = new TimeSpan(0);

            for (var i = 0; i < groups.Count; i++)
            {
                var startTimes = new List<TimeSpan>();
                var endTimes = new List<TimeSpan>();

                for (var j = 0; j < groups[i].WorkshopSequence.Count; j++)
                {
                    if (j == 0)
                    {
                        startTimes.Add(new TimeSpan(0));
                        endTimes.Add(groups[i].WorkshopDurations[j]);
                    }
                    else
                    {
                        startTimes.Add(endTimes[j - 1]);
                        endTimes.Add(startTimes[j] + groups[i].WorkshopDurations[j]);
                    }
                }

                groups[i].WorkshopEndTimes = endTimes;
                groups[i].WorkshopStartTimes = startTimes;

                if (i != 0)
                {
                    TimeSpan maxDiff = new TimeSpan(0);

                    for (var k = 0; k < groups[i].WorkshopSequence.Count; k++)
                    {
                        if (groups[i - 1].WorkshopSequence.Contains(groups[i].WorkshopSequence[k]))
                        {
                            var diff = groups[i - 1].WorkshopEndTimes[groups[i - 1].WorkshopSequence.IndexOf(groups[i].WorkshopSequence[k])] - groups[i].WorkshopStartTimes[k];
                            if (diff > maxDiff)
                                maxDiff = diff;
                        }
                    }

                    for (var f = 0; f < groups[i].WorkshopSequence.Count; f++)
                    {
                        groups[i].WorkshopStartTimes[f] += maxDiff;
                        groups[i].WorkshopEndTimes[f] += maxDiff;
                    }
                }
            }

            productionItemTime = groups.Last().WorkshopEndTimes.Max();

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
    }
}
