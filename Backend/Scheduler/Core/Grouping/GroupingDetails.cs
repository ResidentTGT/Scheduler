using Scheduler.Database;
using Scheduler.Log;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.Grouping
{
    public class GroupingDetails
    {
        private DbManager _dbManager;

        public GroupingDetails(DbManager dbManager)
        {
            _dbManager = dbManager;
        }

        internal void GroupDetails(Order order)
        {
            foreach (var orderQuantum in order.OrderQuantums)
            {
                Logger.Log($"Начато группирование для кванта заказа: изделие: {orderQuantum.ProductionItem.Title}.");
                var groups = new List<ProductionItemQuantumsGroup>();
                foreach (var productionItemQuantum in orderQuantum.ProductionItem.ProductionItemQuantums)
                {
                    if (!groups.Select(g => String.Join(",", g.WorkshopSequence)).Contains(productionItemQuantum.Detail.WorkshopSequenceStr))
                    {
                        groups.Add(new ProductionItemQuantumsGroup()
                        {
                            WorkshopSequence = productionItemQuantum.Detail.WorkshopSequence
                        });
                        Logger.Log($"Добавлена новая группа с последовательностью обработки '{productionItemQuantum.Detail.WorkshopSequenceStr}'");
                    }
                }

                foreach (var group in groups)
                {
                    group.ProductionItemQuantums = orderQuantum.ProductionItem.ProductionItemQuantums.Where(p => p.Detail.WorkshopSequenceStr == String.Join(",", group.WorkshopSequence)).ToList();
                    Logger.Log($"Для группы с последовательностью обработки '{String.Join(",", group.WorkshopSequence)}' найдено {group.ProductionItemQuantums.Count} типов деталей.");
                }

                orderQuantum.ProductionItem.ProductionItemQuantumsGroups = groups;

                Logger.Log($"Закончено группирование для кванта заказа: изделие: {orderQuantum.ProductionItem.Title}, кол-во получившихся групп деталей: {groups.Count}.");
            }

        }

        internal void SetWorkshopSequenceForDetails(Order order)
        {
            foreach (var orderQuantum in order.OrderQuantums)
            {
                var productionItemQuantums = orderQuantum.ProductionItem.ProductionItemQuantums;
                foreach (var productionItemQuantum in productionItemQuantums)
                {
                    var workshopSequence = new HashSet<int>();
                    var equipmentsIdSequence = new List<int>();
                    var equipmentsNameSequence = new List<string>();

                    var sortedOpers = productionItemQuantum.Detail.Routes.First().Operations;

                    foreach (var oper in sortedOpers)
                    {
                        equipmentsIdSequence.Add(oper.Equipment.Id);
                        equipmentsNameSequence.Add(oper.Equipment.Name);
                        workshopSequence.Add((int)oper.Equipment.WorkshopId);
                    }
                    productionItemQuantum.Detail.WorkshopSequence = workshopSequence.ToList();
                    productionItemQuantum.Detail.EquipmentsIdSequence = equipmentsIdSequence;
                    productionItemQuantum.Detail.EquipmentsNameSequence = equipmentsNameSequence;
                }
            }
        }

        internal List<Operation> GetSortedByRouteOperationsByWorkshopId(ProductionItemQuantum productionItemQuantum, int workshopId)
        {
            var opers = new List<Operation>();

            var operationsInWorkshops = productionItemQuantum.Detail.Routes.First().Operations.Where(o => o.Equipment.WorkshopId == workshopId);
             
            var opersSequence = Array.ConvertAll(productionItemQuantum.Detail.Routes.First().OperationsSequence.Split(','), int.Parse).ToList();

            foreach (var operId in opersSequence)
            {
                var oper = operationsInWorkshops.FirstOrDefault(o => o.Id == operId);
                if (oper != null)
                    opers.Add(oper);
            }

            return opers;
        }
    }
}
