using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sorting_App.Data;
using Sorting_App.Models;
using Select_Sort;
using static Azure.Core.HttpHeader;
using System.Net;

namespace Sorting_App.Controllers
{
    /// <summary>
    /// The <see cref="ElementsController"/> class is an MVC <see cref="Controller"/> used to manipulate the <see cref="ElementList"/> model.
    /// </summary>
    public class ElementListsController : Controller
    {
        private readonly SortingContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementListsController"/> class.
        /// </summary>
        /// <param name="context">The <see cref="SortingContext"/> used to query the database.</param>
        public ElementListsController(SortingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gives an IEnumerable to iterate over the list of <see cref="Element"/>s in an <see cref="ElementList"/>, potentially
        /// constrained by the <see cref="ElementTag"/>s on those <see cref="Element"/>s.
        /// </summary>
        /// <param name="elementList">The <see cref="ElementList"/> containing the desired <see cref="Element"/>s.</param>
        /// <param name="tags">A list of strings that reference the desired <see cref="ElementTag"/>s for the <see cref="Element"/>s.
        /// When null, <see cref="GetElements(ElementList, IEnumerable{string}?)"/> returns all the <see cref="Element"/>s in the <see cref="ElementList"/>.</param>
        /// <returns>Returns the desired <see cref="Element"/>s.</returns>
        static public IEnumerable<Element> GetElements(ElementList elementList, IEnumerable<string>? tags)
        {
            Sort? sort = elementList.Sorts.FirstOrDefault();
            IEnumerable<Element> elements;
            if (sort != default && sort.IsSorted)
                elements = SelectSort.GetOrdereredList(sort.SelectElements.ToArray(), sort.ElementComparisons);
            else
                elements = elementList.Elements;

            if (tags == null || !tags.Any())
                return elements;
            else
                return elements.Where(x => x.Tags != null && x.Tags.Any(y => tags.Any(z => y.Tag == z)));
        }

        /// <summary>
        /// Clears the tag constraints on the displayed list of <see cref="Element"/>s.
        /// </summary>
        /// <param name="id">The ID number associated with the <see cref="ElementList"/> being viewed.</param>
        /// <returns>Returns the next View to show the user.</returns>
        public IActionResult ClearTags(int? id)
        {
            TempData.Remove("tags");

            return RedirectToAction("Details", new { id });
        }

        /// <summary>
        /// Gets the next comparison to show the user when sorting an <see cref="ElementList"/>.
        /// </summary>
        /// <param name="sortID">The ID number associated with a <see cref="Sort"/> in the database.</param>
        /// <returns>Returns the next View to show the user.</returns>
        public async Task<IActionResult> Compare(int? sortID)
        {
            if (sortID == null || _context.Sorts == null)
            {
                return NotFound();
            }

            var sort = await _context.Sorts.Include(sort => sort.ElementComparisons).Include(sort => sort.ElementList).FirstOrDefaultAsync(m => m.ID == sortID.Value);
            if (sort == null)
            {
                return NotFound();
            }

            sort.ElementComparisons.RemoveAll(x => x.FirstElement == null || x.SecondElement == null);

            ElementComparison? elementComparison = SelectSort.FindNextComparison(sort.ElementComparisons, sort.SelectionCount);

            if (elementComparison == null)
            {
                sort.IsSorted = true;
                _context.Update(sort);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = sort.ElementList.ID });
            }
            else
                return View(elementComparison);
        }

        /// <summary>
        /// Modifies the constraints for what <see cref="ElementTag"/>s and therefore what <see cref="Element"/>s 
        /// to be shown via <see cref="GetElements(ElementList, IEnumerable{string}?)"/>.
        /// </summary>
        /// <param name="id">The ID number associated with the <see cref="ElementList"/> being constrained.</param>
        /// <param name="tag">The string name associated with an <see cref="ElementTag"/>. If that <see cref="ElementTag"/> is not
        /// already constraining the <see cref="ElementList"/> it will be added. If it is, it will be removed.</param>
        /// <returns>Returns the next View to show the user.</returns>
        public IActionResult Constrain(int? id, string? tag)
        {
            if (tag == null)
            {
                return NotFound();
            }

            List<string> stringTags;

            if (!TempData.ContainsKey("tags"))
                stringTags = new List<string>() { tag };
            else
            {
                stringTags = (TempData["tags"] as IEnumerable<string>)!.ToList();
                if (stringTags.Contains(tag))
                    stringTags.Remove(tag);
                else
                    stringTags.Add(tag);
            }

            TempData["tags"] = stringTags;
            return RedirectToAction("Details", new { id });
        }

        /// <summary>
        /// Gives the MVC View for creating a new <see cref="ElementList"/>.
        /// </summary>
        /// <returns>Returns the next ElementLists/Create  View.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Creates a new <see cref="ElementList"/> based on input from the user.
        /// </summary>
        /// <param name="elementList">The newly created <see cref="ElementList"/> with it's <see cref="ElementList.Name"/> bound.</param>
        /// <param name="image">The image associated with the <see cref="Element"/>, if one is given.</param>
        /// <returns>Returns the next View to show the user.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] ElementList elementList, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                    using (var ms = new MemoryStream())
                    {
                        image.CopyTo(ms);
                        elementList.Image = ms.ToArray();
                    }

                _context.Add(elementList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(elementList);
        }

        /// <summary>
        /// Gives the MVC View for deleting an <see cref="ElementList"/>.
        /// </summary>
        /// <param name="id">The ID number associated with the <see cref="ElementList"/> to be deleted.</param>
        /// <returns>Returns the next View to show the user.</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ElementLists == null)
            {
                return NotFound();
            }

            var elementList = await _context.ElementLists
                .FirstOrDefaultAsync(m => m.ID == id);
            if (elementList == null)
            {
                return NotFound();
            }

            return View(elementList);
        }

        /// <summary>
        /// Deletes the given <see cref="ElementList"/> from the database.
        /// </summary>
        /// <param name="id">The ID number associated with the deleted <see cref="ElementList"/>.</param>
        /// <returns>Returns the next View to show the user.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ElementLists == null)
            {
                return Problem("Entity set 'SortingContext.ElementList'  is null.");
            }
            var elementList = await _context.ElementLists.FindAsync(id);
            if (elementList != null)
            {
                _context.ElementLists.Remove(elementList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Gives the MVC View for showing the details of an <see cref="ElementList"/>.
        /// </summary>
        /// <param name="id">The ID number associated with the <see cref="ElementList"/>.</param>
        /// <returns>Returns the next View to show the user.</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ElementLists == null)
            {
                return NotFound();
            }
            ElementList? elementList = await _context.ElementLists
                .Include(list => list.Elements).ThenInclude(element => element.Tags)
                .Include(list => list.Tags)
                .Include(list => list.Sorts).ThenInclude(sort => sort.ElementComparisons)
                .Include(list => list.Sorts).ThenInclude(sort => sort.SelectElements)
                .AsSplitQuery()
                .FirstOrDefaultAsync(m => m.ID == id.Value);
            if (elementList == null)
            {
                return NotFound();
            }
            IEnumerable<string>? tags;
            if (TempData.ContainsKey("tags"))
                tags = (IEnumerable<string>?)TempData["tags"];
            else
                tags = null;
            return View((elementList, tags));
        }

        /// <summary>
        /// Gives the MVC View for editing the details of an <see cref="ElementList"/>.
        /// </summary>
        /// <param name="id">The ID associated with the <see cref="ElementList"/> to be edited.</param>
        /// <returns>Returns the next View to show the user.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ElementLists == null)
            {
                return NotFound();
            }

            var elementList = await _context.ElementLists.FindAsync(id);
            if (elementList == null)
            {
                return NotFound();
            }
            return View(elementList);
        }

        /// <summary>
        /// Edits an <see cref="ElementList"/> based on input from the user.
        /// </summary>
        /// <param name="id">The ID number associated with the <see cref="ElementList"/> being edited.</param>
        /// <param name="image">The new image associated with the <see cref="Element"/>, if one is given.</param>
        /// <returns>Returns the next View to show the user.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string Name, IFormFile? image)
        {
            if(id == null || _context.ElementLists == null)
            {
                return NotFound();
            }

            var elementList = await _context.ElementLists.FirstOrDefaultAsync(list => list.ID == id);

            if(elementList == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    elementList.Name = Name;
                    if (image != null)
                        using (var ms = new MemoryStream())
                        {
                            image.CopyTo(ms);
                            elementList.Image = ms.ToArray();
                        }

                    _context.Update(elementList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElementListExists(elementList.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(elementList);
        }

        /// <summary>
        /// Gives the MVC View for listing all the <see cref="ElementList"/>s in the database.
        /// </summary>
        /// <returns>Returns the next View to show the user.</returns>
        public async Task<IActionResult> Index()
        {
              return _context.ElementLists != null ? 
                          View(await _context.ElementLists.ToListAsync()) :
                          Problem("Entity set 'SortingContext.ElementList'  is null.");
        }

        /// <summary>
        /// Modifies a <see cref="Sort"/> based on the results from a users choice in comparing two <see cref="Element"/>s.
        /// </summary>
        /// <param name="comparisonID">The ID number associated with the <see cref="ElementComparison"/> whose result is being set.</param>
        /// <param name="degree">The degree signifying the preference for one <see cref="Element"/> over the other in the <see cref="ElementComparison"/>.</param>
        /// <returns>Returns the next View to show the user.</returns>
        public async Task<IActionResult> Results(int? comparisonID, int degree)
        {
            if (comparisonID == null || _context.ElementComparisons == null)
            {
                return NotFound();
            }

            var comparison = await _context.ElementComparisons
                .Include(compare => compare.Sort).ThenInclude(sort => sort.ElementComparisons)
                .Include(compare => compare.Sort).ThenInclude(sort => sort.SelectElements)
                .FirstOrDefaultAsync(m => m.ID == comparisonID.Value);
            if (comparison == null || comparison.Sort == null)
            {
                return NotFound();
            }
            var sort = comparison.Sort;

            sort.ElementComparisons.RemoveAll(x => x.FirstElement == null || x.SecondElement == null);

            SelectSort.SetComparison(comparison.FirstElement, comparison.SecondElement, degree, sort.ElementComparisons);

            SelectSort.AdjustScores(sort.SelectElements,
                SelectSort.GetComparisonMatrix(sort.ElementComparisons, sort.SelectElements.Count)
                );

            _context.Update(sort);
            await _context.SaveChangesAsync();

            return RedirectToAction("Compare", new { sortID = sort.ID });
        }

        /// <summary>
        /// Initializes a new <see cref="Sort"/> and begins the process of having the user sort the <see cref="Element"/>s.
        /// </summary>
        /// <param name="id">The ID number associated witht the <see cref="ElementList"/> being sorted.</param>
        /// <returns>Returns the next View to show the user.</returns>
        public async Task<IActionResult> StartSort(int? id)
        {
            foreach(Sort s in _context.Sorts.Include(e => e.SelectElements).Include(e => e.ElementComparisons))
            {
                _context.RemoveSort(s);
            }

            if (id == null || _context.ElementLists == null)
            {
                return NotFound();
            }

            var elementList = await _context.ElementLists.Include(list => list.Elements).FirstOrDefaultAsync(m => m.ID == id.Value);
            if (elementList == null)
            {
                return NotFound();
            }

            SelectSort.BuildSortDatabase(elementList.Elements, out List<SortingElement> selectElements, out List<ElementComparison> elementComparisons);

            Sort sort = new() {ElementList = elementList, SelectElements = selectElements, ElementComparisons = elementComparisons };

            elementList.Sorts.Add(sort);
            _context.Update(elementList);
            await _context.SaveChangesAsync();

            return RedirectToAction("Compare", new { sortID = sort.ID });
        }

        /// <summary>
        /// Determines if an <see cref="ElementList"/> with the given ID is in the database.
        /// </summary>
        /// <param name="id">The ID number associated with the <see cref="ElementList"/> being searched for.</param>
        /// <returns>Returns true if the <see cref="ElementList"/> is found.</returns>
        private bool ElementListExists(int id)
        {
          return (_context.ElementLists?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
