﻿namespace Sorting_App.Models
{
    public class Sort
    {
        public ElementList ElementList { get; set; }

        public List<ElementComparison> ElementComparisons { get; set; }

        public int ID { get; set; }

        public List<SelectElement> SelectElements { get; set; }

        public int SelectionCount { get; set; }
    }
}