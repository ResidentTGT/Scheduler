using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto
{
    internal static class DtoConverter
    {

        internal static DetailDto ConvertDetail(Detail detail)
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

        internal static Detail ConvertDetail(DetailDto detailDto)
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

        internal static EquipmentDto ConvertEquipment(Equipment equipment)
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
        internal static OrderDto ConvertOrder(Order order)
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
        internal static OrderQuantumDto ConvertOrderQuantum(OrderQuantum orderQuantum)
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

    }
}
