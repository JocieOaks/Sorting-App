﻿using System;
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

namespace Sorting_App.Controllers
{
    public class ElementListsController : Controller
    {
        private readonly SortingContext _context;

        public ElementListsController(SortingContext context)
        {
            _context = context;
        }

        // GET: ElementLists
        public async Task<IActionResult> Index()
        {
              return _context.ElementList != null ? 
                          View(await _context.ElementList.ToListAsync()) :
                          Problem("Entity set 'SortingContext.ElementList'  is null.");
        }

        // GET: ElementLists/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null || _context.ElementList == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index","Elements", new { id });
        }

        // GET: ElementLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ElementLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: ElementLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ElementList == null)
            {
                return NotFound();
            }

            var elementList = await _context.ElementList.FindAsync(id);
            if (elementList == null)
            {
                return NotFound();
            }
            return View(elementList);
        }

        // POST: ElementLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name")] ElementList elementList, IFormFile? image)
        {
            if (id != elementList.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: ElementLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ElementList == null)
            {
                return NotFound();
            }

            var elementList = await _context.ElementList
                .FirstOrDefaultAsync(m => m.ID == id);
            if (elementList == null)
            {
                return NotFound();
            }

            return View(elementList);
        }

        public async Task<IActionResult> StartSort(int? id)
        {
            _context.ElementComparisons.RemoveRange(_context.ElementComparisons);
            _context.SelectElements.RemoveRange(_context.SelectElements);
            _context.Sorts.RemoveRange(_context.Sorts);
            await _context.SaveChangesAsync();

            if (id == null || _context.ElementList == null)
            {
                return NotFound();
            }

            var elementList = await _context.ElementList.Include(list => list.Elements)/*.AsNoTrackingWithIdentityResolution()*/.FirstOrDefaultAsync(m => m.ID == id.Value);
            if (elementList == null)
            {
                return NotFound();
            }

            SelectSort.BuildSortDatabase(elementList.Elements, out List<SelectElement> selectElements, out List<ElementComparison> elementComparisons);

            Sort sort = new() {ElementList = elementList, SelectElements = selectElements, ElementComparisons = elementComparisons };

            elementList.Sorts.Add(sort);
            _context.Update(elementList);
            await _context.SaveChangesAsync();

            return RedirectToAction("Compare", new { id = sort.ID });
        }

        public async Task<IActionResult> Compare(int? id)
        {
            if (id == null || _context.Sorts == null)
            {
                return NotFound();
            }

            var sort = await _context.Sorts.Include(sort => sort.ElementComparisons).FirstOrDefaultAsync(m => m.ID == id.Value);
            if (sort == null)
            {
                return NotFound();
            }

            sort.ElementComparisons.RemoveAll(x => x.FirstElement == null || x.SecondElement == null);

            ElementComparison? elementComparison = SelectSort.FindNextComparison(sort.ElementComparisons, sort.SelectionCount);

            if (elementComparison == null)
            {
                throw new NotImplementedException();
            }
            else
                return View(elementComparison);
        }

        public async Task<IActionResult> Results(int? id, int degree)
        {
            if (id == null || _context.ElementComparisons == null)
            {
                return NotFound();
            }

            var comparison = await _context.ElementComparisons
                .Include(compare => compare.Sort).ThenInclude(sort => sort.ElementComparisons)
                .Include(compare => compare.Sort).ThenInclude(sort => sort.SelectElements)
                .FirstOrDefaultAsync(m => m.ID == id.Value);
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

            return RedirectToAction("Compare", new { id = sort.ID });
        }

        // POST: ElementLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ElementList == null)
            {
                return Problem("Entity set 'SortingContext.ElementList'  is null.");
            }
            var elementList = await _context.ElementList.FindAsync(id);
            if (elementList != null)
            {
                _context.ElementList.Remove(elementList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElementListExists(int id)
        {
          return (_context.ElementList?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}