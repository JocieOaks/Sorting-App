using Microsoft.EntityFrameworkCore;
using Sorting_App.Models;
using System.Drawing;

namespace Sorting_App.Data
{
    /// <summary>
    /// The <see cref="SortingContext"/> class is a <see cref="DbContext"/> that queries for database items in the Sorting App.
    /// </summary>
    public class SortingContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortingContext"/> class.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions"/> used to create this <see cref="SortingContext"/>.</param>
        public SortingContext(DbContextOptions<SortingContext> options) : base(options) { }

        /// <value>The <see cref="ElementComparison"/>s in the database.</value>
        public DbSet<ElementComparison> ElementComparisons { get; set; }

        /// <value>The <see cref="ElementList"/>s in the database.</value>
        public DbSet<ElementList> ElementLists { get; set; }

        /// <value>The <see cref="Element"/>s in the database.</value>
        public DbSet<Element> Elements { get; set; }

        /// <value>The <see cref="SortingElement"/>s in the database.</value>
        public DbSet<SortingElement> SortingElements { get; set; }

        /// <value>The <see cref="Sort"/>s in the database.</value>
        public DbSet<Sort> Sorts { get; set; }

        /// <value>The <see cref="ElementTag"/>s in the database.</value>
        public DbSet<ElementTag> Tags { get; set; }

        /// <summary>
        /// Removes an <see cref="Element"/> from the database, and any corresponding <see cref="ElementComparison"/>s and <see cref="SortingElement"/>.
        /// These are not deleted automatically by the database because the structure of the models creates loops when using ONDELETE CASCADE.
        /// </summary>
        /// <param name="element">The <see cref="Element"/> being removed from the database.</param>
        public void RemoveElement(Element element)
        {
            ElementComparisons.RemoveRange(ElementComparisons.ToList().FindAll(x => x.HasElement(element)));
            SortingElements.RemoveRange(SortingElements.ToList().FindAll(x => x.Element == element));
            Elements.Remove(element);
        }

        /// <summary>
        /// Removes a particular <see cref="Sort"/> and all the associated <see cref="ElementComparison"/>s and <see cref="SortingElement"/>s.
        /// These are not deleted automatically by the database because the structure of the models creates loops when using ONDELETE CASCADE.
        /// </summary>
        /// <param name="sort">The <see cref="Sort"/> being removed from the database.</param>
        public void RemoveSort(Sort sort)
        {
            RemoveRange(sort.ElementComparisons);
            RemoveRange(sort.SelectElements);
            Remove(sort);
        }

        /// <summary>
        /// Checks if an <see cref="ElementTag"/> is actually being used by in any <see cref="Element"/>s in it's corresponding <see cref="ElementList"/>
        /// and removes it from the database if it's not found.
        /// </summary>
        /// <param name="tag">The <see cref="ElementTag"/> being verified.</param>
        public void VerifyTag(ElementTag tag)
        {
            Entry(tag.List).Collection(list => list.Elements).Load();
            foreach (var element in tag.List.Elements)
            {
                Entry(element).Collection(e => e.Tags!).Load();
                if (element.Tags!.Any(x => x == tag))
                    return;
            }
            Tags.Remove(tag);
            SaveChanges();
        }

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.EnableSensitiveDataLogging();
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ElementComparison>().HasOne(e => e.FirstElement).WithMany().IsRequired();
            modelBuilder.Entity<ElementComparison>().HasOne(e => e.SecondElement).WithMany().IsRequired();
            modelBuilder.Entity<ElementComparison>().Navigation(e => e.FirstElement).AutoInclude();
            modelBuilder.Entity<ElementComparison>().Navigation(e => e.SecondElement).AutoInclude();
            modelBuilder.Entity<ElementComparison>().Navigation(e => e.Sort).AutoInclude();
            modelBuilder.Entity<SortingElement>().Navigation(e => e.Element).AutoInclude();
            modelBuilder.Entity<SortingElement>().HasOne(e => e.Element).WithMany().IsRequired();
            modelBuilder.Entity<Sort>().HasMany(e => e.SelectElements).WithOne().IsRequired(false);
            modelBuilder.Entity<Element>().Navigation(e => e.List).AutoInclude();

            base.OnModelCreating(modelBuilder);
        }
    }
}
