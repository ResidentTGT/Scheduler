using Scheduler.Database;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using Scheduler.Core.DecisiveRules;
using Scheduler.Core.CountingTime;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scheduler.Log;

namespace Scheduler.Core.DeterminingOrder
{
    internal class DeterminingGroupsOrder
    {
        private DbManager _dbManager;

        internal DeterminingGroupsOrder(DbManager dbManager)
        {
            _dbManager = dbManager;
        }

        internal void DetermineGroupsOrder(Order order)
        {
            foreach (var orderQuantum in order.OrderQuantums)
            {
                Logger.Log($"Начато определение порядка следования групп. Кол-во групп: {orderQuantum.ProductionItem.ProductionItemQuantumsGroups.Count}", LogLevel.Info);
                var dictDecisiveRulesTimes = new Dictionary<string, TimeSpan>();

                Logger.Log($"Сортировка групп по правилу LUKR начата.", LogLevel.Trace);
                Lukr.SortGroups(orderQuantum.ProductionItem);
                Logger.Log($"Сортировка групп по правилу LUKR закончена.", LogLevel.Trace);
                Logger.Log($"Определение суммарного времени обработки части партии издлий по правилу LUKR начато.", LogLevel.Trace);
                var time = GroupsTiming.CountProductionItemMachiningTime(orderQuantum.ProductionItem, false);
                Logger.Log($"Определение суммарного времени обработки части партии издлий по правилу LUKR закончено. Получившееся время: {time}", LogLevel.Trace);
                dictDecisiveRulesTimes.Add("LUKR", time);

                Logger.Log($"Сортировка групп по правилу SPT начата.", LogLevel.Trace);
                Lukr.SortGroups(orderQuantum.ProductionItem);
                Logger.Log($"Сортировка групп по правилу SPT закончена.", LogLevel.Trace);
                Logger.Log($"Определение суммарного времени обработки части партии издлий по правилу SPT начато.", LogLevel.Trace);
                time = GroupsTiming.CountProductionItemMachiningTime(orderQuantum.ProductionItem, false);
                Logger.Log($"Определение суммарного времени обработки части партии издлий по правилу SPT закончено. Получившееся время: {time}", LogLevel.Trace);
                dictDecisiveRulesTimes.Add("SPT", time);

                Logger.Log($"Выбор и сортировка групп по лучшему правилу начаты.", LogLevel.Info);
                SortByLowestTime(orderQuantum.ProductionItem, dictDecisiveRulesTimes);
                Logger.Log($"Выбор и сортировка групп по лучшему правилу закончены.", LogLevel.Info);

                Logger.Log($"Закончено определение порядка следования групп.", LogLevel.Info);
            }
        }

        private void SortByLowestTime(ProductionItem productionItem, Dictionary<string, TimeSpan> dictDecisiveRulesTimes)
        {
            var rule = dictDecisiveRulesTimes.OrderBy(d => d.Value).First().Key;
            Logger.Log($"Правило с наименьшим временем: {rule}", LogLevel.Info);

            Logger.Log($"Сортировка групп по правилу {rule} начата.", LogLevel.Info);
            switch (rule)
            {
                case "LUKR":
                    Lukr.SortGroups(productionItem);
                    break;
                case "SPT":
                    Spt.SortGroups(productionItem);
                    break;
                default:
                    break;
            }
            Logger.Log($"Сортировка групп по правилу {rule} закончена.", LogLevel.Info);

            Logger.Log($"Определение суммарного времени обработки части партии изделия по лучшему правилу {rule} начато.", LogLevel.Trace);
            var time = GroupsTiming.CountProductionItemMachiningTime(productionItem, true);
            Logger.Log($"Определение суммарного времени обработки части партии изделия по лучшему правилу {rule} закончено. Получившееся время: {time}", LogLevel.Trace);
        }
    }
}
