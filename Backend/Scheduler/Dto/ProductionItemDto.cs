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

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? ParentProductionItemId { get; set; }

        public string ParentProductionItemTitle { get; set; }

        public bool IsNode { get; set; } = false;

        public List<ProductionItemQuantumDto> ProductionItemQuantumsDtos { get; set; }

    }
}
