using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Sorting_App.Data;
using Sorting_App.Models;

namespace Sorting_App.Controllers
{
    public class ElementsController : Controller
    {
        private readonly SortingContext _context;

        public ElementsController(SortingContext context)
        {
            _context = context;
        }

        // GET: Elements
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null || _context.ElementList == null)
            {
                return NotFound();
            }

            ElementList? elementList = await _context.ElementList
                .Include(list => list.Elements).ThenInclude(element => element.Tags)
                .Include(list => list.Tags)
                .FirstOrDefaultAsync(m => m.ID == id.Value);
            if(elementList == null)
            {
                return NotFound();
            }

            return View(elementList);
        }

        // GET: Elements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Elements == null)
            {
                return NotFound();
            }

            var element = await _context.Elements.Include(element => element.Tags)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // GET: Elements/Create
        public async Task<IActionResult> Create(int? id)
        {
            if (id == null || _context.ElementLists == null)
                return NotFound();

            var elementList = await _context.ElementLists
                .FirstOrDefaultAsync(m => m.ID == id);
            if (elementList == null)
            {
                return NotFound();
            }
            return View(new Element() { List = elementList});
        }

        // POST: Elements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,List")] Element element, IFormFile? image, string? tagsString, int? listID)
        {
            if (listID == null || _context.ElementLists == null)
                return NotFound();

            var elementList = await _context.ElementLists.Include(list => list.Elements).Include(list => list.Tags)
                .FirstOrDefaultAsync(m => m.ID == listID);
            if (elementList == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                if(image != null)
                    using(var ms = new MemoryStream())
                    {
                        image.CopyTo(ms);
                        element.Image = ms.ToArray();
                    }

                element.Tags = new();
                if (tagsString != null)
                {
                    foreach (var tagName in tagsString.Split(','))
                    {
                        ElementTag? tag = elementList.Tags.FirstOrDefault(x => x.Tag == tagName);
                        if (tag == default)
                        {
                            tag = new ElementTag() { Tag = tagName, Elements = new List<Element>() { element } };
                            elementList.Tags.Add(tag);
                        }
                        element.Tags.Add(tag);
                    }
                }

                elementList.Elements.Add(element);
                element.List = elementList;
                _context.Add(element);
                _context.Update(elementList);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new {id = listID});
            }
            return View(element);
        }

        // GET: Elements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Elements == null)
            {
                return NotFound();
            }

            var element = await _context.Elements.FindAsync(id);
            if (element == null)
            {
                return NotFound();
            }
            return View(element);
        }

        // POST: Elements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,List")] Element element, IFormFile? image, string? tagsString, int? listID)
        {
            if (id != element.ID)
            {
                return NotFound();
            }
            if (listID == null || _context.ElementLists == null)
                return NotFound();

            var elementList = await _context.ElementLists.Include(list => list.Elements).Include(list => list.Tags)
                .FirstOrDefaultAsync(m => m.ID == listID);
            if (elementList == null)
            {
                return NotFound();
            }
            element.List = elementList;

            if (ModelState.IsValid)
            {
                try
                {
                    if (image != null)
                        using (var ms = new MemoryStream())
                        {
                            image.CopyTo(ms);
                            element.Image = ms.ToArray();
                        }

                    List<ElementTag> contextTags = await _context.Tags.ToListAsync();

                    element.Tags = new();
                    if (tagsString != null)
                    {
                        foreach (var tagName in tagsString.Split(','))
                        {
                            ElementTag? tag = contextTags.FirstOrDefault(x => x.Tag == tagName);
                            if (tag == default)
                            {
                                tag = new ElementTag() { Tag = tagName, Elements = new List<Element>() { element } };
                                _context.Add(tag);
                            }
                            element.Tags.Add(tag);
                        }
                    }

                    _context.Update(element);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElementExists(element.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new {id = listID});
            }
            return View(element);
        }

        // GET: Elements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Elements == null)
            {
                return NotFound();
            }

            var element = await _context.Elements
                .FirstOrDefaultAsync(m => m.ID == id);
            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // POST: Elements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Elements == null)
            {
                return Problem("Entity set 'SortingContext.Elements'  is null.");
            }
            var element = await _context.Elements
                .FirstOrDefaultAsync(x => x.ID == id);

            if (element != null)
            {
                _context.ElementComparisons.RemoveRange(_context.ElementComparisons.ToList().FindAll(x => x.FirstElement.Element == element || x.SecondElement.Element == element));
                _context.SelectElements.RemoveRange(_context.SelectElements.ToList().FindAll(x => x.Element == element));
                _context.Elements.Remove(element);
            }

            await _context.SaveChangesAsync();
            if (element != null)
            {
                return RedirectToAction("Index", new { id = element.List.ID });
            }
            else
                return RedirectToAction("Index", "ElementLists");
        }

        private bool ElementExists(int id)
        {
          return (_context.Elements?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
