using Microsoft.EntityFrameworkCore;
using Sorting_App.Models;
using System.Drawing;

namespace Sorting_App.Data
{
    public class SortingContext : DbContext
    {
        public SortingContext(DbContextOptions<SortingContext> options) : base(options) { }

        public DbSet<ElementComparison> ElementComparisons { get; set; }

        public DbSet<ElementList> ElementLists { get; set; }

        public DbSet<Element> Elements { get; set; }

        public DbSet<SelectElement> SelectElements { get; set; }

        public DbSet<Sort> Sorts { get; set; }

        public DbSet<ElementTag> Tags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.EnableSensitiveDataLogging();
        }

        public void RemoveSort(Sort sort)
        {
            RemoveRange(sort.ElementComparisons);
            RemoveRange(sort.SelectElements);
            Remove(sort);
        }

        public void VerifyTag(ElementTag tag)
        {
            Entry(tag.List).Collection(list => list.Elements).Load();
            foreach(var element in tag.List.Elements)
            {
                Entry(element).Collection(e => e.Tags!).Load();
                if (element.Tags!.Any(x => x == tag))
                    return;
            }
            Tags.Remove(tag);
            SaveChanges();
        }

        public void RemoveElement(Element element)
        {
            ElementComparisons.RemoveRange(ElementComparisons.ToList().FindAll(x => x.HasElement(element)));
            SelectElements.RemoveRange(SelectElements.ToList().FindAll(x => x.Element == element));
            Elements.Remove(element);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ElementComparison>().HasOne(e => e.FirstElement).WithMany().IsRequired();
            modelBuilder.Entity<ElementComparison>().HasOne(e => e.SecondElement).WithMany().IsRequired();
            modelBuilder.Entity<ElementComparison>().Navigation(e => e.FirstElement).AutoInclude();
            modelBuilder.Entity<ElementComparison>().Navigation(e => e.SecondElement).AutoInclude();
            modelBuilder.Entity<ElementComparison>().Navigation(e => e.Sort).AutoInclude();
            modelBuilder.Entity<SelectElement>().Navigation(e => e.Element).AutoInclude();
            modelBuilder.Entity<SelectElement>().HasOne(e => e.Element).WithMany().IsRequired();
            modelBuilder.Entity<Sort>().HasMany(e => e.SelectElements).WithOne().IsRequired(false);
            modelBuilder.Entity<Element>().Navigation(e => e.List).AutoInclude();

            base.OnModelCreating(modelBuilder);
        }
    }
}
