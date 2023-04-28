#nullable disable

namespace Sorting_App.Models
{
    /// <summary>
    /// The <see cref="Sort"/> list is the model containing the data for a user sorting an <see cref="Models.ElementList"/>.
    /// The <see cref="Models.ElementList"/> is sorted by user based on preference or by some other criteria stated by the <see cref="Models.ElementList"/>.
    /// </summary>
    public class Sort
    {
        /// <value>The <see cref="Models.ElementList"/> being sorted.</value>
        public ElementList ElementList { get; set; }

        /// <value>The list of <see cref="ElementComparison"/>s that define the relations between each <see cref="SortingElement"/>.</value>
        public List<ElementComparison> ElementComparisons { get; set; }

        /// <value>The database ID associated with this <see cref="Sort"/>.</value>
        public int ID { get; set; }

        /// <value>The <see cref="SortingElement"/>s created for this <see cref="Sort"/>.</value>
        public List<SortingElement> SelectElements { get; set; }

        /// <value>Determines if all the <see cref="Element"/>s have been sorted, and thus, all the <see cref="ElementComparison"/> are solved.</value>
        public bool IsSorted { get; set; } = false;

        /// <value>The number of comparisons that have been presented to the user to define their preferences.</value>
        public int SelectionCount { get; set; }
    }
}
