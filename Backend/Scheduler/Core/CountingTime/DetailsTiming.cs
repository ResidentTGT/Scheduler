using Scheduler.Core.Grouping;
using Scheduler.Database;
using Scheduler.Log;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.CountingTime
{
    public class DetailsTiming
    {
        private DbManager _dbManager;
        private GroupingDetails _groupingDetails;

        public DetailsTiming(DbManager dbManager)
        {
            _dbManager = dbManager;
            _groupingDetails = new GroupingDetails(_dbManager);
        }

        public TimeSpan CountGroupMachiningTime(ProductionItemQuantumsGroup productionItemQuantumsGroup, bool isFinally, int itemsCountInOnePart)
        {
            var productionItemQuantums = productionItemQuantumsGroup.ProductionItemQuantums;

            var previousGroupLastTime = new TimeSpan(0);

            for (var i = 0; i < productionItemQuantumsGroup.WorkshopSequence.Count; i++)
            {
                var currentWorkshopGroupDurationTime = new TimeSpan(0);

                Logger.Log($"Начат расчет времени группы деталей для цеха с id '{productionItemQuantumsGroup.WorkshopSequence[i]}'", LogLevel.Info);
                for (var j = 0; j < productionItemQuantums.Count; j++)
                {
                    var startTimes = new List<TimeSpan>();
                    var endTimes = new List<TimeSpan>();
                    var durations = new List<TimeSpan>();

                    var operationsInWorkshop = _groupingDetails.GetSortedByRouteOperationsByWorkshopId(productionItemQuantums[j], productionItemQuantumsGroup.WorkshopSequence[i]);

                    var countOfDetailsInOneGroup = productionItemQuantums[j].Count * itemsCountInOnePart;
                    CountTimesForOperationsInWorkshop(operationsInWorkshop, countOfDetailsInOneGroup, startTimes, endTimes, durations);

                    if (j != 0)
                        ShiftTime(productionItemQuantums, operationsInWorkshop, productionItemQuantumsGroup.WorkshopSequence[i], j, startTimes, endTimes);

                    currentWorkshopGroupDurationTime = endTimes.Max();

                    var diffBetween = previousGroupLastTime.Ticks - startTimes.Min().Ticks;
                    if (diffBetween > 0)
                        for (var k = 0; k < startTimes.Count; k++)
                        {
                            startTimes[k] += new TimeSpan(diffBetween);
                            endTimes[k] += new TimeSpan(diffBetween);
                        }

                    foreach (var dur in durations)
                        productionItemQuantums[j].MachiningDurations.Add(dur);
                    foreach (var startTime in startTimes)
                        productionItemQuantums[j].StartTimes.Add(startTime);
                    foreach (var endTime in endTimes)
                        productionItemQuantums[j].EndTimes.Add(endTime);

                    if (productionItemQuantums.Count == (j + 1))
                    {
                        previousGroupLastTime = productionItemQuantums[j].EndTimes.Max();
                        if (isFinally)
                            productionItemQuantumsGroup.WorkshopDurations.Add(currentWorkshopGroupDurationTime);
                    }
                }

                Logger.Log($"Закончен расчет времени группы деталей для цеха с id '{productionItemQuantumsGroup.WorkshopSequence[i]}', " +
               $"суммарное время группы: {currentWorkshopGroupDurationTime}", LogLevel.Info);
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

            return previousGroupLastTime;
        }

        private void ShiftTime(List<ProductionItemQuantum> productionItemQuantums, List<Operation> operationsInWorkshop, int workshopId, int lastProductionItemQuantumIndex, List<TimeSpan> startTimes, List<TimeSpan> endTimes)
        {
            TimeSpan maxDiff = new TimeSpan(0);

            for (var i = 0; i < operationsInWorkshop.Count; i++)
            {
                for (var j = 1; j <= lastProductionItemQuantumIndex; j++)
                {
                    var prevOperationsInWorkshop = _groupingDetails.GetSortedByRouteOperationsByWorkshopId(productionItemQuantums[lastProductionItemQuantumIndex - j], workshopId);

                    var prevDetailOper = prevOperationsInWorkshop.FirstOrDefault(o => o.EquipmentId == operationsInWorkshop[i].EquipmentId);

                    if (prevDetailOper != null)
                    {
                        var diff = productionItemQuantums[lastProductionItemQuantumIndex - j].EndTimes[prevOperationsInWorkshop.IndexOf(prevDetailOper)] - startTimes[i];

                        if (diff > maxDiff)
                            maxDiff = diff;
                        break;
                    }
                }
            }

            for (var i = 0; i < startTimes.Count; i++)
            {
                startTimes[i] += maxDiff;
                endTimes[i] += maxDiff;
            }
        }

        private void CountTimesForOperationsInWorkshop(List<Operation> operationsInWorkshop, int countOfDetailsInOneGroup, List<TimeSpan> startTimes, List<TimeSpan> endTimes, List<TimeSpan> durations)
        {
            for (var k = 0; k < operationsInWorkshop.Count; k++)
            {
                durations.Add(new TimeSpan(operationsInWorkshop[k].MainTime.Ticks * countOfDetailsInOneGroup));

                if (k == 0)
                {
                    startTimes.Add(new TimeSpan(0));
                    endTimes.Add(startTimes[k] + durations[k]);
                }
                else
                {
                    var startTime = startTimes[k - 1] + operationsInWorkshop[k - 1].MainTime;
                    var endTime = startTime + durations[k];
                    if (endTime < endTimes[k - 1] + operationsInWorkshop[k].MainTime)
                    {
                        endTime = endTimes[k - 1] + operationsInWorkshop[k].MainTime;
                        startTime = endTime - durations[k];
                    }
                    startTimes.Add(startTime);
                    endTimes.Add(endTime);
                }
            }
        }
    }
}

