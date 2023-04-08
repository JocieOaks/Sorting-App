using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sorting_App.Models
{
    public class ElementComparison
    {
        public int ID { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public SelectElement FirstElement { get; set; }

        [DeleteBehavior(DeleteBehavior.NoAction)]
        public SelectElement SecondElement { get; set; }

        public Sort Sort { get; set; }

        public int? PreferenceDegree { get; set; }

        public bool Solved => PreferenceDegree.HasValue;

        public int? GetPreference(SelectElement element)
        {
            if (element == FirstElement)
                return PreferenceDegree;
            else
                return -PreferenceDegree;
        }

        public void SetPreference(SelectElement element, int preferenceDegree)
        {
            if (element == FirstElement)
                PreferenceDegree = preferenceDegree;
            else
                PreferenceDegree = -preferenceDegree;
        }

        public bool HasElement(SelectElement element)
        {
            return element == FirstElement || element == SecondElement;
        }

        public bool HasElement(Element element)
        {
            return element == FirstElement.Element || element == SecondElement.Element;
        }

        public bool HasElements(SelectElement element1, SelectElement element2)
        {
            return element1 == FirstElement && element2 == SecondElement || element2 == FirstElement && element1 == SecondElement;
        }
    }
}
