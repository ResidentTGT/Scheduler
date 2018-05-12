using Scheduler.Core.Grouping;
using Scheduler.Database;
using Scheduler.Dto.Reporting;
using Scheduler.Model;
using Scheduler.Model.OrderReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto
{
    internal class DtoConverter
    {
        private DbManager _dbManager;

        public DtoConverter()
        {
            _dbManager = new DbManager();
        }
        #region ConvertDetails
        internal DetailDto ConvertDetail(Detail detail)
        {
            var detailDto = new DetailDto()
            {
                Cost = detail.Cost,
                Description = detail.Description,
                Id = detail.Id,
                IsPurchased = detail.IsPurchased,
                RouteName = detail.Routes.FirstOrDefault() != null ? detail.Routes.First().Name : null,
                Title = detail.Title,
                WorkshopSequence = detail.WorkshopSequence
            };

            return detailDto;
        }

        internal DetailDto ConvertDetailForViewing(Detail detail)
        {
            var detailDto = new DetailDto()
            {
                Cost = detail.Cost,
                Description = detail.Description,
                Id = detail.Id,
                IsPurchased = detail.IsPurchased,
                RouteName = detail.Routes.First() != null ? detail.Routes.First().Name : null,
                Title = detail.Title,
                WorkshopSequence = detail.WorkshopSequence,
                EquipmentsIdSequence = detail.EquipmentsIdSequence,
                EquipmentsNameSequence = detail.EquipmentsNameSequence
            };

            return detailDto;
        }

        internal Detail ConvertDetail(DetailDto detailDto)
        {
            var detail = new Detail()
            {
                Cost = detailDto.Cost,
                Description = detailDto.Description,
                IsPurchased = detailDto.IsPurchased,
                WorkshopSequence = detailDto.WorkshopSequence != null ? detailDto.WorkshopSequence : new List<int>(),
                Title = detailDto.Title
            };
            if (detailDto.Id.HasValue)
                detail.Id = (int)detailDto.Id;

            return detail;
        }
        #endregion

        #region EquipmentConvert

        internal EquipmentDto ConvertEquipment(Equipment equipment)
        {
            var equipmentDto = new EquipmentDto()
            {
                Description = equipment.Description,
                Id = equipment.Id,
                Name = equipment.Name,
                Type = equipment.Type,
                Conveyor = equipment.Conveyor != null ? ConvertConveyor(equipment.Conveyor) : null,
                Workshop = equipment.Workshop != null ? ConvertWorkshop(equipment.Workshop) : null,
                Cost = equipment.Cost,
                LoadFactor = equipment.LoadFactor,
                MaintenanceCost = equipment.MaintenanceCost,
                UsingTimeResource = equipment.UsingTimeResource
            };

            return equipmentDto;
        }

        internal Equipment ConvertEquipment(EquipmentDto equipmentDto)
        {
            var equipment = new Equipment()
            {
                Description = equipmentDto.Description,
                Name = equipmentDto.Name,
                Type = equipmentDto.Type,
                ConveyorId = equipmentDto.Conveyor != null ? equipmentDto.Conveyor.Id : null,
                WorkshopId = equipmentDto.Workshop != null ? equipmentDto.Workshop.Id : null,
                Cost = equipmentDto.Cost,
                LoadFactor = equipmentDto.LoadFactor,
                MaintenanceCost = equipmentDto.MaintenanceCost,
                UsingTimeResource = equipmentDto.UsingTimeResource
            };

            return equipment;
        }
        #endregion

        #region OrderConvert
        internal OrderDto ConvertOrder(Order order)
        {
            var orderDto = new OrderDto()
            {
                Id = order.Id,
                Description = order.Description,
                Name = order.Name,
                PlannedBeginDate = order.PlannedBeginDate,
                PlannedEndDate = order.PlannedEndDate,
                State = order.State,
                OrderQuantumsCount = order.OrderQuantums.Count()
            };

            return orderDto;
        }
        internal Order ConvertOrder(OrderDto orderDto)
        {
            var order = new Order()
            {

                Description = orderDto.Description,
                Name = orderDto.Name,
                PlannedBeginDate = orderDto.PlannedBeginDate,
                PlannedEndDate = orderDto.PlannedEndDate,
                State = orderDto.State,
                OrderQuantums = orderDto.OrderQuantums != null ? orderDto.OrderQuantums.Select(d => ConvertOrderQuantum(d)).ToList() : null,
            };

            return order;
        }
        internal OrderDto ConvertOrderForViewing(Order order)
        {
            var orderDto = new OrderDto()
            {
                Id = order.Id,
                Description = order.Description,
                Name = order.Name,
                PlannedBeginDate = order.PlannedBeginDate,
                PlannedEndDate = order.PlannedEndDate,
                State = order.State,
                OrderQuantums = order.OrderQuantums != null ? order.OrderQuantums.Select(d => ConvertOrderQuantumForViewing(d)).ToList() : null,
            };

            return orderDto;
        }



        #endregion

        #region Reporting
        internal OrderReportDto ConvertOrderReportForView(OrderReport orderReport)
        {
            var orderReportDto = new OrderReportDto()
            {
                CreationTime = orderReport.CreationTime,
                Id = orderReport.Id,
                OrderBlocks = orderReport.OrderBlocks.Select(b => ConvertOrderBlockForView(b)).ToList()
            };

            return orderReportDto;
        }

        internal OrderBlockDto ConvertOrderBlockForView(OrderBlock block)
        {
            var blockDto = new OrderBlockDto()
            {
                Id = block.Id,
                IsMachining = block.IsMachining,
                ProductionItemId = block.ProductionItemId,
                ProductionItemsCount = block.ProductionItemsCount,
                ProductionItemsName = block.ProductionItemsName,
                Duration = block.Duration,
                StartTime = block.StartTime,
                GroupBlocks = block.GroupBlocks.Select(b => ConvertGroupBlockForView(b)).ToList()
            };

            return blockDto;
        }

        internal DetailsBatchBlockDto ConvertDetailsBatchBlockForView(DetailsBatchBlock block)
        {
            var blockDto = new DetailsBatchBlockDto()
            {
                Id = block.Id,
                DetailId = block.DetailId,
                DetailName = block.DetailName,
                DetailsCount = block.DetailsCount,
                Duration = block.Duration,
                StartTime = block.StartTime,
                Equipment = ConvertEquipment(block.Equipment)
            };

            return blockDto;
        }

        internal GroupBlockDto ConvertGroupBlockForView(GroupBlock block)
        {
            var blockDto = new GroupBlockDto()
            {
                Id = block.Id,
                Duration = block.Duration,
                StartTime = block.StartTime,
                GroupIndex = block.GroupIndex,
                Workshop = ConvertWorkshop(block.Workshop),
                DetailsBatchBlocks = block.DetailsBatchBlocks.Select(b => ConvertDetailsBatchBlockForView(b)).ToList()
            };

            return blockDto;
        }
        #endregion

        #region OrderQuantumConvert
        internal OrderQuantumDto ConvertOrderQuantum(OrderQuantum orderQuantum)
        {
            var orderQuantumDto = new OrderQuantumDto()
            {
                Id = orderQuantum.Id,
                Count = orderQuantum.Count,
                ItemsCountInOnePart = orderQuantum.ItemsCountInOnePart,
                OrderId = orderQuantum.OrderId,
                ProductionItem = orderQuantum.ProductionItem != null ? ConvertProductionItem(orderQuantum.ProductionItem) : null,
            };

            return orderQuantumDto;
        }
        internal OrderQuantum ConvertOrderQuantum(OrderQuantumDto orderQuantumDto)
        {
            var orderQuantum = new OrderQuantum()
            {
                Count = orderQuantumDto.Count,
                ItemsCountInOnePart = orderQuantumDto.ItemsCountInOnePart,
            };
            if (orderQuantumDto.ProductionItem.Id != null)
                orderQuantum.ProductionItemId = (int)orderQuantumDto.ProductionItem.Id;

            return orderQuantum;
        }
        internal OrderQuantumDto ConvertOrderQuantumForViewing(OrderQuantum orderQuantum)
        {
            var orderQuantumDto = new OrderQuantumDto()
            {
                Id = orderQuantum.Id,
                Count = orderQuantum.Count,
                ItemsCountInOnePart = orderQuantum.ItemsCountInOnePart,
                OrderId = orderQuantum.OrderId,
                ProductionItem = orderQuantum.ProductionItem != null ? ConvertProductionItemForViewing(orderQuantum.ProductionItem) : null,
                AssemblingDurations = orderQuantum.AssemblingDurations.Select(a => a.Ticks).ToList(),
                AssemblingEndTimes = orderQuantum.AssemblingEndTimes.Select(a => a.Ticks).ToList(),
                AssemblingFullBatchTime = orderQuantum.AssemblingFullBatchTime.Ticks,
                AssemblingFullPartTime = orderQuantum.AssemblingFullPartTime.Ticks,
                AssemblingRemainingFromPartsTime = orderQuantum.AssemblingRemainingFromPartsTime.HasValue ? orderQuantum.AssemblingRemainingFromPartsTime.Value.Ticks : default(long),
                AssemblingStartTimes = orderQuantum.AssemblingStartTimes.Select(a => a.Ticks).ToList(),
                MachiningDurations = orderQuantum.MachiningDurations.Select(a => a.Ticks).ToList(),
                MachiningEndTimes = orderQuantum.MachiningEndTimes.Select(a => a.Ticks).ToList(),
                MachiningFullPartTime = orderQuantum.MachiningFullPartTime.Ticks,
                MachiningRemainingFromPartsTime = orderQuantum.MachiningRemainingFromPartsTime.Ticks,
                MachiningStartTimes = orderQuantum.MachiningStartTimes.Select(a => a.Ticks).ToList()
            };

            return orderQuantumDto;
        }


        #endregion

        #region ProductionItemConvert
        internal ProductionItemDto ConvertProductionItem(ProductionItem productionItem)
        {
            var productionItemDto = new ProductionItemDto()
            {
                Id = productionItem.Id,
                Title = productionItem.Title,
                Description = productionItem.Description,
                ProductionItemQuantums = productionItem.ProductionItemQuantums != null ? productionItem.ProductionItemQuantums.Select(d => ConvertProductionItemQuantum(d)).ToList() : null
            };

            return productionItemDto;
        }

        internal ProductionItemDto ConvertProductionItemForView(ProductionItem productionItem)
        {
            var productionItemDto = new ProductionItemDto()
            {
                Id = productionItem.Id,
                Title = productionItem.Title,
                Description = productionItem.Description,
                DetailsCount = productionItem.ProductionItemQuantums.Count,
                ChildrenProductionItemsCount = productionItem.ChildrenProductionItemsIds.Length == 0 ? 0 : productionItem.ChildrenProductionItemsIds.Split(',').Length,
                OneItemIncome = productionItem.OneItemIncome
            };

            return productionItemDto;
        }

        internal ProductionItem ConvertProductionItem(ProductionItemDto productionItemDto)
        {
            var productionItem = new ProductionItem()
            {
                Title = productionItemDto.Title,
                Description = productionItemDto.Description,
                ChildrenProductionItemsIds = String.Join(",", productionItemDto.AddingItems.Where(p => p.Type == ProductDto.ProductType.ProductionItem).Select(p => p.Id.ToString()).ToArray()),
                ProductionItemQuantums = productionItemDto.AddingItems.Where(p => p.Type == ProductDto.ProductType.Detail)
                .Select(d => ConvertProductDto(d)).ToList(),
                OneItemIncome = productionItemDto.OneItemIncome
            };

            return productionItem;
        }
        internal ProductionItemDto ConvertProductionItemForViewing(ProductionItem productionItem)
        {
            var productionItemDto = new ProductionItemDto()
            {
                Id = productionItem.Id,
                Title = productionItem.Title,
                Description = productionItem.Description,

                ProductionItemQuantums = productionItem.ProductionItemQuantums != null ? productionItem.ProductionItemQuantums.Select(d => ConvertProductionItemQuantumForViewing(d)).ToList() : null,
                ProductionItemQuantumsGroups = productionItem.ProductionItemQuantumsGroups.Select(d => ConvertProductionItemQuantumsGroupForViewing(d)).ToList()
            };

            return productionItemDto;
        }
        internal ProductionItemQuantumsGroupDto ConvertProductionItemQuantumsGroupForViewing(ProductionItemQuantumsGroup productionItemQuantumsGroup)
        {
            var productionItemQuantumsGroupDto = new ProductionItemQuantumsGroupDto()
            {
                ProductionItemQuantums = productionItemQuantumsGroup.ProductionItemQuantums.Select(p => ConvertProductionItemQuantumForViewing(p)).ToList(),
                WorkshopDurations = productionItemQuantumsGroup.WorkshopDurations.Select(a => a.Ticks).ToList(),
                WorkshopEndTimes = productionItemQuantumsGroup.WorkshopEndTimes.Select(a => a.Ticks).ToList(),
                WorkshopSequence = productionItemQuantumsGroup.WorkshopSequence,
                WorkshopStartTimes = productionItemQuantumsGroup.WorkshopStartTimes.Select(a => a.Ticks).ToList()
            };

            return productionItemQuantumsGroupDto;
        }
        #endregion

        #region ProductionItemQuantumConvert
        internal ProductionItemQuantumDto ConvertProductionItemQuantum(ProductionItemQuantum productionItemQuantum)
        {
            var productionItemQuantumDto = new ProductionItemQuantumDto()
            {
                Count = productionItemQuantum.Count,
                Detail = ConvertDetail(productionItemQuantum.Detail),
                Id = productionItemQuantum.Id,
                ProductionItemId = productionItemQuantum.ProductionItemId
            };

            return productionItemQuantumDto;
        }
        internal ProductionItemQuantumDto ConvertProductionItemQuantumForViewing(ProductionItemQuantum productionItemQuantum)
        {
            var productionItemQuantumDto = new ProductionItemQuantumDto()
            {
                Count = productionItemQuantum.Count,
                Detail = ConvertDetailForViewing(productionItemQuantum.Detail),
                Id = productionItemQuantum.Id,
                ProductionItemId = productionItemQuantum.ProductionItemId,
                EndTimes = productionItemQuantum.EndTimes.Select(a => a.Ticks).ToList(),
                MachiningDurations = productionItemQuantum.MachiningDurations.Select(a => a.Ticks).ToList(),
                StartTimes = productionItemQuantum.StartTimes.Select(a => a.Ticks).ToList()
            };

            return productionItemQuantumDto;
        }

        internal ProductionItemQuantum ConvertProductionItemQuantum(ProductionItemQuantumDto productionItemQuantumDto)
        {
            var productionItemQuantum = new ProductionItemQuantum()
            {
                Count = productionItemQuantumDto.Count,
                Detail = ConvertDetail(productionItemQuantumDto.Detail),
            };

            return productionItemQuantum;
        }
        internal ProductionItemQuantum ConvertProductDto(ProductDto productDto)
        {
            var productionItemQuantum = new ProductionItemQuantum()
            {
                Count = productDto.Count,
                DetailId = productDto.Id,
            };

            return productionItemQuantum;
        }
        #endregion

        #region OperationConvert
        internal OperationDto ConvertOperation(Operation operation)
        {
            var operationDto = new OperationDto()
            {
                Id = operation.Id,
                Name = operation.Name,
                MainTime = operation.MainTime.Ticks,
                AdditionalTime = operation.AdditionalTime.Ticks,
                Description = operation.Description,
                Type = operation.Type,
                Detail = operation.Detail != null ? ConvertDetail(operation.Detail) : null,
                Equipment = operation.Equipment != null ? ConvertEquipment(operation.Equipment) : null,
                RiggingCost = operation.RiggingCost,
                RiggingStorageCost = operation.RiggingStorageCost
            };

            return operationDto;
        }

        internal Operation ConvertOperation(OperationDto operationDto)
        {
            var operation = new Operation()
            {
                Name = operationDto.Name,
                MainTime = new TimeSpan(operationDto.MainTime),
                AdditionalTime = new TimeSpan(operationDto.AdditionalTime),
                Description = operationDto.Description,
                Type = operationDto.Type,
                DetailId = operationDto.Detail.Id,
                EquipmentId = operationDto.Equipment.Id,
                RiggingCost = operationDto.RiggingCost,
                RiggingStorageCost = operationDto.RiggingStorageCost
            };
            if (operationDto.Id.HasValue)
                operation.Id = (int)operationDto.Id;

            return operation;
        }
        #endregion

        #region RouteConvert
        internal RouteDto ConvertRoute(Route route)
        {
            var routeDto = new RouteDto()
            {
                Id = route.Id,
                Description = route.Description,
                Detail = ConvertDetail(route.Detail),
                Name = route.Name,
                AssemblingOperationsCount = route.Operations.Where(o => o.Type == OperationType.Assembling).Count(),
                MachiningOperationsCount = route.Operations.Where(o => o.Type == OperationType.Machining).Count(),
                TransportOperationsCount = route.Operations.Where(o => o.Type == OperationType.Transport).Count(),
            };

            return routeDto;
        }

        internal Route ConvertRoute(RouteDto routeDto)
        {
            var route = new Route()
            {
                Description = routeDto.Description,
                Id = (int)routeDto.Detail.Id,
                Name = routeDto.Name,
                Operations = routeDto.Operations.ToList().Select(o => ConvertOperation(o)).ToList(),
                OperationsSequence = String.Join(",", routeDto.OperationsSequence),
                DetailId = routeDto.Detail.Id
            };

            return route;
        }
        #endregion

        #region ConvertConveyorsAndWorkshops
        internal WorkshopDto ConvertWorkshop(Workshop workshop)
        {
            var workshopDto = new WorkshopDto()
            {
                Id = workshop.Id,
                Name = workshop.Name,
                Description = workshop.Description,
                EquipmentsCount = workshop.Equipments.Count
            };

            return workshopDto;
        }
        internal Workshop ConvertWorkshop(WorkshopDto workshopDto)
        {
            var workshop = new Workshop()
            {
                Name = workshopDto.Name,
                Description = workshopDto.Description
            };
            return workshop;
        }
        internal ConveyorDto ConvertConveyor(Conveyor conveyor)
        {
            var conveyorDto = new ConveyorDto()
            {
                Id = conveyor.Id,
                Name = conveyor.Name,
                Description = conveyor.Description,
                EquipmentsCount = conveyor.Equipments.Count
            };

            return conveyorDto;
        }
        internal Conveyor ConvertConveyor(ConveyorDto conveyorDto)
        {
            var conveyor = new Conveyor()
            {
                Name = conveyorDto.Name,
                Description = conveyorDto.Description
            };
            return conveyor;
        }
        #endregion


    }
}
