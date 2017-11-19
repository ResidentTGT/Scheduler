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

        internal DetailDto ConvertDetail(Detail detail)
        {
            var detailDto = new DetailDto()
            {
                Cost = detail.Cost,
                Description = detail.Description,
                Id = detail.Id,
                IsPurchased = detail.IsPurchased,
                RouteId = detail.RouteId,
                Title = detail.Title
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
                RouteId = detailDto.RouteId,
                Title = detailDto.Title
            };

            return detail;
        }

        #region EquipmentConvert

        internal EquipmentDto ConvertEquipment(Equipment equipment)
        {
            var equipmentDto = new EquipmentDto()
            {
                Description = equipment.Description,
                Id = equipment.Id,
                Name = equipment.Name,
                Type = equipment.Type
            };

            return equipmentDto;
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
                OrderQuantumsDtos = order.OrderQuantums.Select(d => ConvertOrderQuantum(d)).ToList()
            };

            return orderDto;
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
                ProductionItemId = orderQuantum.ProductionItemId,
                ProductionItemTitle = orderQuantum.ProductionItem.Title
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
                IsNode = productionItem.IsNode,
                ParentProductionItemId = productionItem.ParentProductionItemId,
                ParentProductionItemTitle = productionItem.ParentProductionItemId.HasValue ? _dbManager.GetProductionItemById(productionItem.ParentProductionItemId).Title : "",
                ProductionItemQuantumsDtos = productionItem.ProductionItemQuantums.Select(d => ConvertProductionItemQuantum(d)).ToList()
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
                ParentProductionItemId = productionItemDto.ParentProductionItemId,

                //ProductionItemQuantums = productionItemDto.ProductionItemQuantumsDtos.Select(d => ConvertProductionItemQuantum(d)).ToList()
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
                DetailId = productionItemQuantum.DetailId,
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
                DetailId = productionItemQuantumDto.DetailId,
            };

            return productionItemQuantum;
        }
        #endregion
    }
}
