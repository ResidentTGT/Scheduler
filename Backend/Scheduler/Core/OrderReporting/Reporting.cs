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
                        GroupBlocks = GenerateGroupBlocks(orderQuantum.ProductionItem.ProductionItemQuantumsGroups, i, orderQuantum.MachiningStartTimes[i])
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

        private List<GroupBlock> GenerateGroupBlocks(List<ProductionItemQuantumsGroup> groups, int index, TimeSpan orderBlockStartTime)
        {
            var groupBlocks = new List<GroupBlock>();
            var workshops = _dbManager.GetWorkshops().ToList();

            for (int k = 0; k < groups.Count; k++)
            {
                var groupStartTime = new TimeSpan(groups[k].WorkshopStartTimes[0].Ticks + orderBlockStartTime.Ticks);

                for (int j = 0; j < groups[k].WorkshopSequence.Count; j++)
                {
                    long transportTime = 0;
                    if (j != 0)
                        transportTime = groups[k].TransportOperations[j - 1].Duration.Ticks;

                    groupBlocks.Add(new GroupBlock()
                    {
                        Duration = groups[k].WorkshopDurations[j].Ticks,
                        GroupIndex = k,
                        StartTime = groups[k].WorkshopStartTimes[j].Ticks + orderBlockStartTime.Ticks,
                        WorkshopId = groups[k].WorkshopSequence[j],
                        DetailsBatchBlocks = GenerateDetailsBatchBlocks(groups[k], groups[k].WorkshopSequence[j], groupStartTime + new TimeSpan(transportTime)),
                        TransportOperationBlock = new TransportOperationBlock()
                        {
                            Distance = groups[k].TransportOperations[j].Distance,
                            Duration = groups[k].TransportOperations[j].Duration.Ticks,
                            FirstWorkshopId = groups[k].TransportOperations[j].FirstWorkshopId,
                            FirstWorkshopName = workshops.FirstOrDefault(w => w.Id == groups[k].TransportOperations[j].FirstWorkshopId).Name,
                            SecondWorkshopId = groups[k].TransportOperations[j].SecondWorkshopId,
                            SecondWorkshopName = groups[k].TransportOperations[j].SecondWorkshop != null ? workshops.FirstOrDefault(w => w.Id == groups[k].TransportOperations[j].SecondWorkshopId).Name : "Конвейер"
                        }
                    });
                }
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
