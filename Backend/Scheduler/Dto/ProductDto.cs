using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ProductType Type { get; set; }

        public int Count { get; set; }

        public enum ProductType
        {
            Detail = 1,
            ProductionItem = 2
        }
    }
}
