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
        private List<Transport> _transports;

        internal DeterminingGroupsOrder(DbManager dbManager, List<Transport> transports)
        {
            _dbManager = dbManager;
            _transports = transports;
        }

        internal void DetermineGroupsOrder(Order order)
        {
            foreach (var orderQuantum in order.OrderQuantums)
            {
                Logger.Log($"Начато определение порядка следования групп. Кол-во групп: {orderQuantum.ProductionItem.ProductionItemQuantumsGroups.Count}", LogLevel.Info);
                var dictDecisiveRulesTimes = new Dictionary<string, TimeSpan>();

                var groupsTiming = new GroupsTiming(_transports, _dbManager);

                Logger.Log($"Сортировка групп по правилу LUKR начата.", LogLevel.Trace);
                Lukr.SortGroups(orderQuantum.ProductionItem);
                Logger.Log($"Сортировка групп по правилу LUKR закончена.", LogLevel.Trace);
                Logger.Log($"Определение суммарного времени обработки части партии издлий по правилу LUKR начато.", LogLevel.Trace);
                var time = groupsTiming.CountProductionItemMachiningTime(orderQuantum, false);
                Logger.Log($"Определение суммарного времени обработки части партии издлий по правилу LUKR закончено. Получившееся время: {time}", LogLevel.Trace);
                dictDecisiveRulesTimes.Add("LUKR", time);

                Logger.Log($"Сортировка групп по правилу SPT начата.", LogLevel.Trace);
                Lukr.SortGroups(orderQuantum.ProductionItem);
                Logger.Log($"Сортировка групп по правилу SPT закончена.", LogLevel.Trace);
                Logger.Log($"Определение суммарного времени обработки части партии издлий по правилу SPT начато.", LogLevel.Trace);
                time = groupsTiming.CountProductionItemMachiningTime(orderQuantum, false);
                Logger.Log($"Определение суммарного времени обработки части партии издлий по правилу SPT закончено. Получившееся время: {time}", LogLevel.Trace);
                dictDecisiveRulesTimes.Add("SPT", time);

                Logger.Log($"Сортировка групп по правилу LPT начата.", LogLevel.Trace);
                Lukr.SortGroups(orderQuantum.ProductionItem);
                Logger.Log($"Сортировка групп по правилу LPT закончена.", LogLevel.Trace);
                Logger.Log($"Определение суммарного времени обработки части партии издлий по правилу LPT начато.", LogLevel.Trace);
                time = groupsTiming.CountProductionItemMachiningTime(orderQuantum, false);
                Logger.Log($"Определение суммарного времени обработки части партии издлий по правилу LPT закончено. Получившееся время: {time}", LogLevel.Trace);
                dictDecisiveRulesTimes.Add("LPT", time);

                Logger.Log($"Сортировка групп по правилу ReverseLukr начата.", LogLevel.Trace);
                Lukr.SortGroups(orderQuantum.ProductionItem);
                Logger.Log($"Сортировка групп по правилу ReverseLukr закончена.", LogLevel.Trace);
                Logger.Log($"Определение суммарного времени обработки части партии издлий по правилу ReverseLukr начато.", LogLevel.Trace);
                time = groupsTiming.CountProductionItemMachiningTime(orderQuantum, false);
                Logger.Log($"Определение суммарного времени обработки части партии издлий по правилу ReverseLukr закончено. Получившееся время: {time}", LogLevel.Trace);
                dictDecisiveRulesTimes.Add("ReverseLukr", time);



                Logger.Log($"Выбор и сортировка групп по лучшему правилу начаты.", LogLevel.Trace);
                SortByLowestTime(orderQuantum, dictDecisiveRulesTimes);
                Logger.Log($"Выбор и сортировка групп по лучшему правилу закончены.", LogLevel.Trace);

                Logger.Log($"Закончено определение порядка следования групп.", LogLevel.Info);
            }
        }

        private void SortByLowestTime(OrderQuantum orderQuantum, Dictionary<string, TimeSpan> dictDecisiveRulesTimes)
        {
            var rule = dictDecisiveRulesTimes.OrderBy(d => d.Value).First().Key;
            Logger.Log($"Правило с наименьшим временем: {rule}", LogLevel.Trace);

            Logger.Log($"Сортировка групп по правилу {rule} начата.", LogLevel.Trace);
            switch (rule)
            {
                case "LUKR":
                    Lukr.SortGroups(orderQuantum.ProductionItem);
                    break;
                case "SPT":
                    Spt.SortGroups(orderQuantum.ProductionItem);
                    break;
                case "LPT":
                    Spt.SortGroups(orderQuantum.ProductionItem);
                    break;
                case "ReverseLukr":
                    Lukr.SortGroups(orderQuantum.ProductionItem);
                    break;
                default:
                    break;
            }
            Logger.Log($"Сортировка групп по правилу {rule} закончена.", LogLevel.Trace);

            Logger.Log($"Определение суммарного времени обработки части партии изделия по лучшему правилу {rule} начато.", LogLevel.Trace);
            var groupsTiming = new GroupsTiming(_transports, _dbManager);
            var time = groupsTiming.CountProductionItemMachiningTime(orderQuantum, true);
            Logger.Log($"Определение суммарного времени обработки части партии изделия по лучшему правилу {rule} закончено. Получившееся время: {time}", LogLevel.Trace);
        }
    }
}
