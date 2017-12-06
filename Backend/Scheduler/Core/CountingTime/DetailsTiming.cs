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

                    var opers = productionItemQuantums[j].Detail.Route.Operations.Where(o => o.Equipment.WorkshopId == productionItemQuantumsGroup.WorkshopSequence[i]).ToList();

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
                            durations.Add(new TimeSpan(opers[k].MainTime.Ticks * productionItemQuantums[j].Count * itemsCountInOnePart));
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

                    if (j != 0)
                    {
                        TimeSpan maxDiff = new TimeSpan(0);

                        for (var p = 0; p < opers.Count; p++)
                        {
                            for (var piqIndex = 1; piqIndex <= j; piqIndex++)
                            {
                                var prevDetailOper = productionItemQuantums[j - piqIndex].Detail.Route.Operations.FirstOrDefault(o => o.EquipmentId == opers[p].EquipmentId);
                                if (prevDetailOper != null)
                                {
                                    var diff = productionItemQuantums[j - piqIndex]
                                        .EndTimes[productionItemQuantums[j - piqIndex].Detail.Route.Operations.Where(o => o.Equipment.WorkshopId == productionItemQuantumsGroup.WorkshopSequence[i]).ToList().IndexOf(prevDetailOper)]
                                        - startTimes[p];
                                    if (diff > maxDiff)
                                        maxDiff = diff;
                                    break;
                                }
                            }


                        }
                        for (var f = 0; f < startTimes.Count; f++)
                        {
                            startTimes[f] += maxDiff;
                            endTimes[f] += maxDiff;
                        }
                    }

                    var diffBetween = groupTime.Ticks-startTimes.First().Ticks ;
                    if (diffBetween > 0)
                        for (var f = 0; f < startTimes.Count; f++)
                        {
                            startTimes[f] += new TimeSpan(diffBetween);
                            endTimes[f] += new TimeSpan(diffBetween);
                        }

                    foreach (var dur in durations)
                        productionItemQuantums[j].MachiningDurations.Add(dur);
                    foreach (var startTime in startTimes)
                        productionItemQuantums[j].StartTimes.Add(startTime);
                    foreach (var endTime in endTimes)
                        productionItemQuantums[j].EndTimes.Add(endTime);

                    if (isFinally)
                        productionItemQuantumsGroup.WorkshopDurations.Add(endTimes.Last() - startTimes.First());

                    groupTime = productionItemQuantums[j].EndTimes.Max();
                }

                Logger.Log($"Закончен расчет времени группы деталей для цеха с id '{productionItemQuantumsGroup.WorkshopSequence[i]}', " +
               $"суммарное время: {groupTime}", LogLevel.Info);
            }



            if (!isFinally)
            {
                foreach (var d in productionItemQuantums)
                {
                    d.EndTimes.Clear();
                    d.StartTimes.Clear();
                    d.MachiningDurations.Clear();
                }
            }



            return groupTime;
        }

    }
}

