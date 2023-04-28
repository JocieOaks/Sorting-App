using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Sorting_App.Models
{
    /// <summary>
    /// The <see cref="ElementComparison"/> class is the model of the relationship between two <see cref="SortingElement"/>s in a <see cref="Models.Sort"/>.
    /// </summary>
    public class ElementComparison
    {
        /// <value>The first of the two <see cref="SortingElement"/>s being compared.</value>
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public SortingElement FirstElement { get; set; }

        /// <value>The database ID for the <see cref="ElementComparison"/>.</value>
        public int ID { get; set; }

        /// <value>The degree to which <see cref="FirstElement"/> is preferred to <see cref="SecondElement"/>.
        /// Can be null if the relation between the two <see cref="SortingElement"/>s is unknown.</value>
        public int? PreferenceDegree { get; set; }

        /// <value>The first of the two <see cref="SortingElement"/>s being compared.</value>
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public SortingElement SecondElement { get; set; }

        /// <value>Determines if the comparison between the two <see cref="SortingElement"/>s is known.</value>
        public bool Solved => PreferenceDegree.HasValue;

        /// <value>The <see cref="Models.Sort"/> that contains this <see cref="ElementComparison"/>.</value>
        public Sort Sort { get; set; }

        /// <summary>
        /// Get's the preference of the given <see cref="SortingElement"/> over it's paired <see cref="SortingElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="SortingElement"/> whose preference is being evaluated.</param>
        /// <returns>Returns the preference of <c>element</c>. Can be null if the preference is unknown.</returns>
        /// <exception cref="ArgumentException">Thrown if <c>element</c> is not one of the <see cref="SortingElement"/>s in this <see cref="ElementComparison"/>.</exception>
        public int? GetPreference(SortingElement element)
        {
            if (!HasElement(element))
                throw new ArgumentException("Given Element is not a part of this ElementComparison.");
            if (element == FirstElement)
                return PreferenceDegree;
            else
                return -PreferenceDegree;
        }

        /// <summary>
        /// Determines if a given <see cref="SortingElement"/> is part of the <see cref="ElementComparison"/>.
        /// </summary>
        /// <param name="element">The <see cref="SortingElement"/> being evaluated.</param>
        /// <returns>Returns true if <c>element</c> is one of the two <see cref="SortingElement"/>s being compared.</returns>
        public bool HasElement(SortingElement element)
        {
            return element == FirstElement || element == SecondElement;
        }

        /// <summary>
        /// Determines if a given <see cref="Element"/> and its associated <see cref="SortingElement"/> is part of the <see cref="ElementComparison"/>.
        /// </summary>
        /// <param name="element">The <see cref="Element"/> being evaluated.</param>
        /// <returns>Returns true if <c>element</c> is one of the two <see cref="Element"/>s being compared.</returns>
        public bool HasElement(Element element)
        {
            return element == FirstElement.Element || element == SecondElement.Element;
        }

        /// <summary>
        /// Determines if the <see cref="ElementComparison"/> compares two given <see cref="SortingElement"/>s.
        /// </summary>
        /// <param name="element1">The first <see cref="SortingElement"/>.</param>
        /// <param name="element2">The second <see cref="SortingElement"/>.</param>
        /// <returns>Returns true if the two <see cref="SortingElement"/>s are <see cref="FirstElement"/> and <see cref="SecondElement"/>.</returns>
        public bool HasElements(SortingElement element1, SortingElement element2)
        {
            return element1 == FirstElement && element2 == SecondElement || element2 == FirstElement && element1 == SecondElement;
        }

        /// <summary>
        /// Set's the preference of the given <see cref="SortingElement"/> over it's paired <see cref="SortingElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="SortingElement"/> whose preference is being set.</param>
        /// <param name="preferenceDegree">The degree to which <c>element</c> is prefered.</param>
        /// <exception cref="ArgumentException">Thrown if <c>element</c> is not one of the <see cref="SortingElement"/>s in this <see cref="ElementComparison"/>.</exception>
        public void SetPreference(SortingElement element, int preferenceDegree)
        {
            if (!HasElement(element))
                throw new ArgumentException("Given Element is not a part of this ElementComparison.");
            if (element == FirstElement)
                PreferenceDegree = preferenceDegree;
            else
                PreferenceDegree = -preferenceDegree;
        }
    }
}
