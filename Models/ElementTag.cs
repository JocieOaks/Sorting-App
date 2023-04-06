namespace Sorting_App.Models
{
    /// <summary>
    /// The <see cref="ElementTag"/> class is the model for tags associated with an <see cref="Element"/>.
    /// </summary>
    public class ElementTag
    {
        /// <value>The database ID for the <see cref="ElementTag"/>.</value>
        public int ID { get; set; }

        /// <value>The name of the <see cref="ElementTag"/>.</value>
        public string Tag { get; set; } = "";

        /// <value>The list of all <see cref="Element"/>s that have this <see cref="ElementTag"/>.</value>
        public List<Element>? Elements { get; set; }

        /// <value>The <see cref="ElementList"/> that contains <see cref="Element"/>s with this <see cref="ElementTag"/>.</value>
        public ElementList List { get; set; }
    }
}
