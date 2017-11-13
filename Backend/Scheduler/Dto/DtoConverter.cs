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

    }
}
