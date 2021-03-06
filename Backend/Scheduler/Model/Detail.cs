﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler.Model
{
    public class Detail
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? Cost { get; set; }

        public bool? IsPurchased { get; set; }

        public virtual List<Route> Routes { get; set; }

        public virtual List<ProductionItem> ProductionItems { get; set; }

        public virtual List<ProductionItemQuantum> ProductionItemQuantums { get; set; }

        public virtual List<Operation> Operations { get; set; }

        /// <summary>
        /// см
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// см
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// см
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// кг
        /// </summary>
        public int Mass { get; set; }

        public List<int> WorkshopSequence
        {
            get
            {
                return String.IsNullOrEmpty(WorkshopSequenceStr)
                    ? new List<int>()
                    : WorkshopSequenceStr.Split(',').Select(w => Convert.ToInt32(w)).ToList();
            }
            set
            {
                WorkshopSequenceStr = String.Join(",", value.Select(v => Convert.ToString(v)));
            }
        }

        public string WorkshopSequenceStr { get; set; }
        [NotMapped]
        public List<int> EquipmentsIdSequence { get; set; }
        [NotMapped]
        public List<string> EquipmentsNameSequence { get; set; }
    }
}
