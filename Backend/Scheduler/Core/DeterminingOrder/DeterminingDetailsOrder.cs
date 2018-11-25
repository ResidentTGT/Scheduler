using Scheduler.Core.CountingTime;
using Scheduler.Core.DecisiveRules;
using Scheduler.Core.Grouping;
using Scheduler.Database;
using Scheduler.Log;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.DeterminingOrder
{
    public class DeterminingDetailsOrder
    {
        private DbManager _dbManager;

        public DeterminingDetailsOrder(DbManager dbManager)
        {
            _dbManager = dbManager;
        }

        internal void DetermineDetailsOrderInGroups(Order order)
        {
            foreach (var orderQuantum in order.OrderQuantums)
            {
                foreach (var productionItemQuantumsGroup in orderQuantum.ProductionItem.ProductionItemQuantumsGroups)
                {
                    Logger.Log($"Начато определение порядка обработки деталей с последовательностью следования по цехам '{String.Join(", ", productionItemQuantumsGroup.WorkshopSequence)}'", LogLevel.Info);
                    var dictDecisiveRulesTimes = new Dictionary<string, TimeSpan>();
                    var detailsTiming = new DetailsTiming(_dbManager);

                    Logger.Log($"Сортировка деталей по правилу LUKR начата.", LogLevel.Trace);
                    Lukr.SortDetails(productionItemQuantumsGroup);
                    Logger.Log($"Сортировка деталей по правилу LUKR закончена.", LogLevel.Trace);
                    Logger.Log($"Определение суммарного времени обработки группы по правилу LUKR начато.", LogLevel.Trace);
                    var time = detailsTiming.CountGroupMachiningTime(productionItemQuantumsGroup, false, orderQuantum.ItemsCountInOnePart);
                    Logger.Log($"Определение суммарного времени обработки группы по правилу LUKR закончено. Получившееся время: {time}", LogLevel.Trace);
                    dictDecisiveRulesTimes.Add("LUKR", time);

                    Logger.Log($"Сортировка деталей по правилу SPT начата.", LogLevel.Trace);
                    Spt.SortDetails(productionItemQuantumsGroup);
                    Logger.Log($"Сортировка деталей по правилу SPT закончена.", LogLevel.Trace);
                    Logger.Log($"Определение суммарного времени обработки группы по правилу SPT начато.", LogLevel.Trace);
                    time = detailsTiming.CountGroupMachiningTime(productionItemQuantumsGroup, false, orderQuantum.ItemsCountInOnePart);
                    Logger.Log($"Определение суммарного времени обработки группы по правилу SPT закончено. Получившееся время: {time}", LogLevel.Trace);
                    dictDecisiveRulesTimes.Add("SPT", time);

                    Logger.Log($"Сортировка деталей по правилу LPT начата.", LogLevel.Trace);
                    Lpt.SortDetails(productionItemQuantumsGroup);
                    Logger.Log($"Сортировка деталей по правилу LPT закончена.", LogLevel.Trace);
                    Logger.Log($"Определение суммарного времени обработки группы по правилу LPT начато.", LogLevel.Trace);
                    time = detailsTiming.CountGroupMachiningTime(productionItemQuantumsGroup, false, orderQuantum.ItemsCountInOnePart);
                    Logger.Log($"Определение суммарного времени обработки группы по правилу LPT закончено. Получившееся время: {time}", LogLevel.Trace);
                    dictDecisiveRulesTimes.Add("LPT", time);

                    Logger.Log($"Сортировка деталей по правилу ReverseLukr начата.", LogLevel.Trace);
                    ReverseLukr.SortDetails(productionItemQuantumsGroup);
                    Logger.Log($"Сортировка деталей по правилу ReverseLukr закончена.", LogLevel.Trace);
                    Logger.Log($"Определение суммарного времени обработки группы по правилу ReverseLukr начато.", LogLevel.Trace);
                    time = detailsTiming.CountGroupMachiningTime(productionItemQuantumsGroup, false, orderQuantum.ItemsCountInOnePart);
                    Logger.Log($"Определение суммарного времени обработки группы по правилу ReverseLukr закончено. Получившееся время: {time}", LogLevel.Trace);
                    dictDecisiveRulesTimes.Add("ReverseLukr", time);

                    Logger.Log($"Выбор и сортировка деталей по лучшему правилу начаты.", LogLevel.Trace);
                    SortByLowestTime(detailsTiming,productionItemQuantumsGroup, dictDecisiveRulesTimes, orderQuantum.ItemsCountInOnePart);
                    Logger.Log($"Выбор и сортировка деталей по лучшему правилу закончены.", LogLevel.Trace);

                    Logger.Log($"Закончено определение порядка обработки деталей с последовательностью следования по цехам '{String.Join(", ", productionItemQuantumsGroup.WorkshopSequence)}'", LogLevel.Info);
                }
            }

        }

        private void SortByLowestTime(DetailsTiming detailsTiming,ProductionItemQuantumsGroup productionItemQuantumsGroup, Dictionary<string, TimeSpan> dictDecisiveRulesTimes, int itemsCountInOnePart)
        {
            var rule = dictDecisiveRulesTimes.OrderBy(d => d.Value).First().Key;
            Logger.Log($"Правило с наименьшим временем: {rule}", LogLevel.Trace);

            Logger.Log($"Сортировка деталей по правилу {rule} начата.", LogLevel.Trace);
            switch (rule)
            {
                case "LUKR":
                    Lukr.SortDetails(productionItemQuantumsGroup);
                    break;
                case "SPT":
                    Spt.SortDetails(productionItemQuantumsGroup);
                    break;
                case "LPT":
                    Lpt.SortDetails(productionItemQuantumsGroup);
                    break;
                case "ReverseLukr":
                    Lukr.SortDetails(productionItemQuantumsGroup);
                    break;
                default:
                    break;
            }
            Logger.Log($"Сортировка деталей по правилу {rule} закончена.", LogLevel.Trace);

            Logger.Log($"Определение суммарного времени обработки группы по лучшему правилу {rule} начато.", LogLevel.Trace);
            var time = detailsTiming.CountGroupMachiningTime(productionItemQuantumsGroup, true, itemsCountInOnePart);
            Logger.Log($"Определение суммарного времени обработки группы по лучшему правилу {rule} закончено. Получившееся время: {time}", LogLevel.Trace);
        }
    }
}
