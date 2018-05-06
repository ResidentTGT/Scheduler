using Scheduler.Core.Grouping;
using Scheduler.Database;
using Scheduler.Model;
using Scheduler.Model.OrderReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.OrderReporting
{
    internal class Reporting
    {
        private DbManager _dbManager;

        internal Reporting(DbManager dbManager)
        {
            _dbManager = dbManager;
        }

        internal OrderReport CreateReport(Order order)
        {
            var report = new OrderReport
            {
                OrderId = order.Id
            };

            var orderBlocks = GenerateOrderBlocks(order);

            report.OrderBlocks = orderBlocks;
            report.CreationTime = DateTime.Now;

            return report;
        }

        private List<OrderBlock> GenerateOrderBlocks(Order order)
        {
            var orderBlocks = new List<OrderBlock>();

            foreach (var orderQuantum in order.OrderQuantums)
            {
                for (int i = 0; i < orderQuantum.MachiningDurations.Count; i++)
                {
                    orderBlocks.Add(new OrderBlock()
                    {
                        Duration = orderQuantum.MachiningDurations[i].Ticks,
                        IsMachining = true,
                        ProductionItemId = orderQuantum.ProductionItemId,
                        ProductionItemsCount = orderQuantum.ItemsCountInOnePart,
                        ProductionItemsName = orderQuantum.ProductionItem.Title,
                        StartTime = orderQuantum.MachiningStartTimes[i].Ticks,
                        GroupBlocks = GenerateGroupBlock(orderQuantum.ProductionItem, i)
                    });
                }

                for (int i = 0; i < orderQuantum.AssemblingDurations.Count; i++)
                {
                    orderBlocks.Add(new OrderBlock()
                    {
                        Duration = orderQuantum.AssemblingDurations[i].Ticks,
                        IsMachining = false,
                        ProductionItemId = orderQuantum.ProductionItemId,
                        ProductionItemsCount = orderQuantum.ItemsCountInOnePart,
                        ProductionItemsName = orderQuantum.ProductionItem.Title,
                        StartTime = orderQuantum.AssemblingStartTimes[i].Ticks
                    });
                }
            }

            return orderBlocks;
        }

        private List<GroupBlock> GenerateGroupBlock(ProductionItem productionItem, int index)
        {
            var groupBlocks = new List<GroupBlock>();

            var groups = productionItem.ProductionItemQuantumsGroups;
            for (int i = 0; i < groups.Count; i++)
            {
                for (int j = 0; j < groups[i].WorkshopSequence.Count; j++)
                {
                    groupBlocks.Add(new GroupBlock()
                    {
                        Duration = groups[i].WorkshopDurations[j].Ticks,
                        GroupIndex = i,
                        StartTime = groups[i].WorkshopStartTimes[j].Ticks,
                        WorkshopId = groups[i].WorkshopSequence[j],
                        DetailsBatchBlocks = GenerateDetailsBatchBlocks(groups[i], groups[i].WorkshopSequence[j])
                    });
                }
            }

            return groupBlocks;
        }

        private List<DetailsBatchBlock> GenerateDetailsBatchBlocks(ProductionItemQuantumsGroup productionItemQuantumsGroup, int workshopId)
        {
            var detailsBatchBlocks = new List<DetailsBatchBlock>();

            var items = productionItemQuantumsGroup.ProductionItemQuantums;
            for (int i = 0; i < items.Count; i++)
            {
                for (int j = 0; j < items[i].Detail.EquipmentsIdSequence.Count; j++)
                {
                    detailsBatchBlocks.Add(new DetailsBatchBlock()
                    {
                        DetailId = items[i].Detail.Id,
                        DetailName = items[i].Detail.Title,
                        DetailsCount = items[i].Count,
                        Duration = items[i].MachiningDurations[j].Ticks,
                        EquipmentId = items[i].Detail.EquipmentsIdSequence[j],
                        StartTime = items[i].StartTimes[j].Ticks
                    });
                }
            }

            return detailsBatchBlocks;
        }
    }
}
