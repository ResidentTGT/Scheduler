using Scheduler.Core.Grouping;
using Scheduler.Log;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.CountingTime
{
    public static class DetailsTiming
    {
        public static TimeSpan CountGroupMachiningTime(ProductionItemQuantumsGroup productionItemQuantumsGroup, bool isFinally, int itemsCountInOnePart)
        {
            var productionItemQuantums = productionItemQuantumsGroup.ProductionItemQuantums;

            var groupTime = new TimeSpan(0);

            for (var i = 0; i < productionItemQuantumsGroup.WorkshopSequence.Count; i++)
            {
                Logger.Log($"Начат расчет времени группы деталей для цеха с id '{productionItemQuantumsGroup.WorkshopSequence[i]}'", LogLevel.Info);
                for (var j = 0; j < productionItemQuantums.Count; j++)
                {
                    var startTimes = new List<TimeSpan>();
                    var endTimes = new List<TimeSpan>();
                    var durations = new List<TimeSpan>();

                    var opers = productionItemQuantums[j].Detail.Operations.Where(o => o.Equipment.WorkshopId == productionItemQuantumsGroup.WorkshopSequence[i]).ToList();

                    for (var k = 0; k < opers.Count; k++)
                    {
                        if (k == 0)
                        {
                            durations.Add(new TimeSpan(opers[k].MainTime.Ticks * productionItemQuantums[j].Count * itemsCountInOnePart));
                            startTimes.Add(new TimeSpan(0));
                            endTimes.Add(startTimes[k] + durations[k]);
                        }
                        else
                        {
                            durations.Add(new TimeSpan(opers[k].MainTime.Ticks * productionItemQuantums[j].Count* itemsCountInOnePart));
                            var startTime = startTimes[k - 1] + opers[k].MainTime;
                            var endTime = startTime + durations[k];
                            if (endTime < endTimes[k - 1] + opers[k].MainTime)
                            {
                                endTime = endTimes[k - 1] + opers[k].MainTime;
                                startTime = endTime - durations[k];
                            }
                            startTimes.Add(startTime);
                            endTimes.Add(endTime);
                        }
                    }
                    productionItemQuantums[j].MachiningDurations = durations;
                    productionItemQuantums[j].StartTimes = startTimes;
                    productionItemQuantums[j].EndTimes = endTimes;

                    if (j != 0)
                    {
                        TimeSpan maxDiff = new TimeSpan(0);

                        for (var p = 0; p < opers.Count; p++)
                        {
                            var prevDetailOper = productionItemQuantums[j - 1].Detail.Operations.FirstOrDefault(o => o.EquipmentId == opers[p].EquipmentId);
                            if (prevDetailOper != null)
                            {
                                var diff = productionItemQuantums[j - 1]
                                    .EndTimes[productionItemQuantums[j - 1].Detail.Operations.Where(o => o.Equipment.WorkshopId == productionItemQuantumsGroup.WorkshopSequence[i]).ToList().IndexOf(prevDetailOper)] 
                                    - productionItemQuantums[j].StartTimes[p];
                                if (diff > maxDiff)
                                    maxDiff = diff;
                            }
                        }
                        for (var f = 0; f < productionItemQuantums[j].StartTimes.Count; f++)
                        {
                            productionItemQuantums[j].StartTimes[f] += maxDiff;
                            productionItemQuantums[j].EndTimes[f] += maxDiff;
                        }

                    }
                }

                var maxEndTime = productionItemQuantums.Last().EndTimes.Max();
                groupTime += maxEndTime;
                if (isFinally)
                    productionItemQuantumsGroup.WorkshopDurations.Add(maxEndTime);
                else
                {
                    foreach (var d in productionItemQuantums)
                    {
                        d.EndTimes.Clear();
                        d.StartTimes.Clear();
                        d.MachiningDurations.Clear();
                    }
                }

                Logger.Log($"Закончен расчет времени группы деталей для цеха с id '{productionItemQuantumsGroup.WorkshopSequence[i]}', " +
                    $"суммарное время: {maxEndTime}", LogLevel.Info);
            }
            return groupTime;
        }

    }
}

