using Scheduler.Database;
using Scheduler.Model;
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
                RouteName = detail.Route != null ? detail.Route.Name : null,
                Title = detail.Title,
                WorkshopSequence = detail.WorkshopSequence
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
                Workshop = equipment.Workshop != null ? ConvertWorkshop(equipment.Workshop) : null
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
                WorkshopId = equipmentDto.Workshop != null ? equipmentDto.Workshop.Id : null
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
                OrderQuantums = order.OrderQuantums != null ? order.OrderQuantums.Select(d => ConvertOrderQuantum(d)).ToList() : null
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


        #endregion


        #region ProductionItemConvert
        internal ProductionItemDto ConvertProductionItem(ProductionItem productionItem)
        {
            var productionItemDto = new ProductionItemDto()
            {
                Id = productionItem.Id,
                Title = productionItem.Title,
                Description = productionItem.Description,
                IsNode = productionItem.IsNode,
                ParentProductionItemId = productionItem.ParentProductionItemId,
                ParentProductionItemTitle = productionItem.ParentProductionItemId.HasValue ? _dbManager.GetProductionItemById(productionItem.ParentProductionItemId).Title : "",
                ProductionItemQuantums = productionItem.ProductionItemQuantums != null ? productionItem.ProductionItemQuantums.Select(d => ConvertProductionItemQuantum(d)).ToList() : null
            };

            return productionItemDto;
        }

        internal ProductionItem ConvertProductionItem(ProductionItemDto productionItemDto)
        {
            var productionItem = new ProductionItem()
            {
                Title = productionItemDto.Title,
                Description = productionItemDto.Description,
                IsNode = productionItemDto.IsNode,
                ParentProductionItemId = productionItemDto.ParentProductionItemId.HasValue ? productionItemDto.ParentProductionItemId : null,
                ProductionItemQuantums = productionItemDto.ProductionItemQuantums != null ? productionItemDto.ProductionItemQuantums.Select(d => ConvertProductionItemQuantum(d)).ToList() : null
            };

            return productionItem;
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

        internal ProductionItemQuantum ConvertProductionItemQuantum(ProductionItemQuantumDto productionItemQuantumDto)
        {
            var productionItemQuantum = new ProductionItemQuantum()
            {
                Count = productionItemQuantumDto.Count,
                Detail = ConvertDetail(productionItemQuantumDto.Detail),
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
                Equipment = operation.Equipment != null ? ConvertEquipment(operation.Equipment) : null
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
                EquipmentId = operationDto.Equipment.Id
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
                Operations = route.Operations.Select(o => ConvertOperation(o)).ToList()
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
                Operations = routeDto.Operations.ToList().Select(o => ConvertOperation(o)).ToList()
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
                Name = workshop.Name
            };

            return workshopDto;
        }
        internal Workshop ConvertWorkshop(WorkshopDto workshopDto)
        {
            var workshop = new Workshop()
            {
                Name = workshopDto.Name
            };
            return workshop;
        }
        internal ConveyorDto ConvertConveyor(Conveyor conveyor)
        {
            var conveyorDto = new ConveyorDto()
            {
                Id = conveyor.Id,
                Name = conveyor.Name
            };

            return conveyorDto;
        }
        internal Conveyor ConvertConveyor(ConveyorDto conveyorDto)
        {
            var conveyor = new Conveyor()
            {
                Name = conveyorDto.Name
            };
            return conveyor;
        }
        #endregion


    }
}
