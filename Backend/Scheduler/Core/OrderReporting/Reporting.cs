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

        internal OrderReport GenerateReport(Order order)
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
                        GroupBlocks = orderQuantum.ProductionItem.ProductionItemQuantumsGroups.Count > 1
                        ? GenerateGroupBlock(orderQuantum.ProductionItem.ProductionItemQuantumsGroups[0], i, orderQuantum.MachiningStartTimes[i])
                        : GenerateGroupBlock(orderQuantum.ProductionItem.ProductionItemQuantumsGroups[0], i, orderQuantum.MachiningStartTimes[i])
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

        private List<GroupBlock> GenerateGroupBlock(ProductionItemQuantumsGroup group, int index, TimeSpan startTime)
        {
            var groupBlocks = new List<GroupBlock>();

            for (int j = 0; j < group.WorkshopSequence.Count; j++)
            {
                groupBlocks.Add(new GroupBlock()
                {
                    Duration = group.WorkshopDurations[j].Ticks,
                    GroupIndex = index,
                    StartTime = group.WorkshopStartTimes[j].Ticks + startTime.Ticks,
                    WorkshopId = group.WorkshopSequence[j],
                    DetailsBatchBlocks = GenerateDetailsBatchBlocks(group, group.WorkshopSequence[j], startTime)
                });
            }

            return groupBlocks;
        }

        private List<DetailsBatchBlock> GenerateDetailsBatchBlocks(ProductionItemQuantumsGroup productionItemQuantumsGroup, int workshopId, TimeSpan startTime)
        {
            var detailsBatchBlocks = new List<DetailsBatchBlock>();

            var items = productionItemQuantumsGroup.ProductionItemQuantums;
            for (int i = 0; i < items.Count; i++)
            {
               // var equipmentsInWorkshop = _dbManager.GetEquipmentsInWorkshop(workshopId).Select(e => e.Id);
                for (int j = 0; j < items[i].MachiningDurations.Count; j++)
                {
                    detailsBatchBlocks.Add(new DetailsBatchBlock()
                    {
                        DetailId = items[i].Detail.Id,
                        DetailName = items[i].Detail.Title,
                        DetailsCount = items[i].Count,
                        Duration = items[i].MachiningDurations[j].Ticks,
                        EquipmentId = items[i].Detail.EquipmentsIdSequence[j],
                        StartTime = items[i].StartTimes[j].Ticks + startTime.Ticks
                    });
                }
            }

            return detailsBatchBlocks;
        }
    }
}
