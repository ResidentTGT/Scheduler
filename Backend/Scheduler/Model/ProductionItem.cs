using Scheduler.Core.Grouping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public class ProductionItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ChildrenProductionItemsIds { get; set; }

        public virtual List<ProductionItemQuantum> ProductionItemQuantums { get; set; }

        public virtual List<OrderQuantum> OrderQuantums { get; set; }

        /// <summary>
        /// Не входит в схему БД. Нужно для группирования деталей.
        /// </summary>
        [NotMapped]
        public List<ProductionItemQuantumsGroup> ProductionItemQuantumsGroups { get; set; }
    }
}
