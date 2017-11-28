using Scheduler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Core.Grouping
{
    public class ProductionItemQuantumsGroup
    {
        public List<ProductionItemQuantum> ProductionItemQuantums { get; set; }

        /// <summary>
        /// Последовательность прохождения группы деталей цехов, значение - id цеха.
        /// </summary>
        public List<int> WorkshopSequence { get; set; }

        /// <summary>
        /// Времена обработки группы деталей в цехах, последовательность совпадает с последовательностью прохождения цехов.
        /// </summary>
        public List<TimeSpan> WorkshopStartTimes { get; set; } = new List<TimeSpan>();
        public List<TimeSpan> WorkshopEndTimes { get; set; } = new List<TimeSpan>();
        public List<TimeSpan> WorkshopDurations { get; set; } = new List<TimeSpan>();
        
    }
}
