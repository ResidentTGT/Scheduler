using Scheduler.Core.Grouping;
using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto
{
    public class ProductionItemDto
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int DetailsCount { get; set; }

        public int ChildrenProductionItemsCount { get; set; }

        public int OneItemIncome { get; set; }

        public List<ProductDto> AddingItems { get; set; }

        public List<ProductionItemQuantumDto> ProductionItemQuantums { get; set; }

        public List<ProductionItemQuantumsGroupDto> ProductionItemQuantumsGroups { get; set; }
    }
}
