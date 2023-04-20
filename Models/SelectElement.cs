using Select_Sort;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Sorting_App.Models
{
    /// <summary>
    /// The <see cref="SelectElement"/> class is the model containing data for an associated <see cref="Models.Element"/> being sorted by a <see cref="Sort"/>. 
    /// </summary>
    public class SelectElement
    {
        /// <summary>
        /// Intitailizes a new instance of the <see cref="SelectElement"/> class.
        /// </summary>
        /// <param name="element">The element being sorted by <see cref="Sort"/>.</param>
        public SelectElement(Element element, int index)
        {
            Element = element;
            Index = index;
        }

        /// <summary>
        /// Initializes a new instace of the <see cref="SelectElement"/> class without parameters.
        /// </summary>
        public SelectElement() { }

        /// <value>The <see cref="Models.Element"/> being sorted.</value>
        public Element Element { get; private set; }

        /// <value>The database ID associated with this <see cref="SelectElement"/>.</value>
        public int ID { get; private set; }

        /// <value>The index of the <see cref="SelectElement"/> in a <see cref="Sort"/>.</value>
        public int Index { get; private set; }

        /// <value>The last time that <see cref="Element"/> was presented as an option, based on the count of options that have been shown.</value>
        public int LastTimeAsOption { get; private set; } = -1000;

        /// <value>The number of times the user has seen <see cref="Element"/> as an option for comparison.</value>
        public int NumberOfTimesAsOption { get; private set; } = 0;

        /// <value>The amount to change <see cref="Score"/> by. Used to wait until all <see cref="SelectElement"/>'s have been evaluated so that their mutual
        /// force can be applied symmetrically.</value>
        public float Push { get; set; }

        /// <value>The estimated score of the <see cref="Element"/>, where all <see cref="SelectElement"/>s with a higher score are ranked higher than
        /// <see cref="Element"/> and all <see cref="SelectElement"/>s with a lower score are ranked lower than <see cref="Element"/>.</value>
        public float Score { get; private set; }

        /// <summary>
        /// Evaluates the fatigue a user has from seeing an option repeatedly, in such a way that can affect their ranking.
        /// </summary>
        /// <param name="selectionCount">The current number of selections that have been made.</param>
        /// <returns>Returns the fatigue score.</returns>
        public int Fatigue(int selectionCount)
        {
            int recency = 5 + LastTimeAsOption - selectionCount;
            if (recency > 0)
                return NumberOfTimesAsOption + recency;
            else
                return NumberOfTimesAsOption;
        }

        /// <summary>
        /// Changes <see cref="Score"/> based on <see cref="Push"/>.
        /// </summary>
        public void OnPush()
        {
            Score += Push;
        }

        /// <summary>
        /// Called when <see cref="SelectElement"/> has been shown as an option to the user.
        /// </summary>
        /// <param name="selectionCount">The current number of selections that have been made.</param>
        public void OptionPresented(int selectionCount)
        {
            NumberOfTimesAsOption++;
            LastTimeAsOption = selectionCount;
        }
    }
}
