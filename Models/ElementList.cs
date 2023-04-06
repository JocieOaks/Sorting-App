namespace Sorting_App.Models
{
    public class ElementList
    {
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;

        public byte[]? Image { get; set; }

        public List<Element> Elements { get; set; } = new List<Element>();

        public List<ElementTag> Tags { get; set; } = new List<ElementTag>();

        public List<Sort> Sorts { get; set; } = new List<Sort>();
    }
}
