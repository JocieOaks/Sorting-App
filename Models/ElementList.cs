namespace Sorting_App.Models
{
    /// <summary>
    /// The <see cref="ElementList"/> class is the model for a group of user given <see cref="Element"/>s.
    /// </summary>
    public class ElementList
    {
        /// <value>The list of <see cref="Element"/>s in the <see cref="ElementList"/>.</value>
        public List<Element> Elements { get; set; } = new List<Element>();

        /// <value>The database ID for the <see cref="ElementList"/>.</value>
        public int ID { get; set; }

        /// <value>An image, converted into a byte array that is associated with this <see cref="ElementList"/>. Can be null.</value>
        public byte[]? Image { get; set; }

        /// <value>The name of the <see cref="ElementList"/> that is shown to the user.</value>
        public string Name { get; set; } = string.Empty;

        /// <value>The list of all the <see cref="Sort"/>s that users have used to put the <see cref="ElementList"/> in order.</value>
        public List<Sort> Sorts { get; set; } = new List<Sort>();

        /// <value>The list of all <see cref="ElementTag"/>s that the <see cref="ElementList"/>'s <see cref="Element"/>s might have.</value>
        public List<ElementTag> Tags { get; set; } = new List<ElementTag>();
    }
}
