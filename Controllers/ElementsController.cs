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
using Select_Sort;
using Sorting_App.Migrations;
using System.Xml.Linq;
using Azure;

namespace Sorting_App.Controllers
{
    public class ElementsController : Controller
    {
        private readonly SortingContext _context;

        public ElementsController(SortingContext context)
        {
            _context = context;
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

                GetTags(tagsString, element, elementList);

                elementList.Elements.Add(element);
                element.List = elementList;
                _context.Add(element);
                _context.Update(elementList);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "ElementLists", new {id = listID});
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
        public async Task<IActionResult> Edit(int? id, string Name, IFormFile? image, string? tagsString, int? listID)
        {
            if (id == null || _context.Elements == null)
            {
                return NotFound();
            }
            
            var element = await _context.Elements
                .Include(element => element.Tags)
                .Include(element => element.List).ThenInclude(list => list.Tags)
                .FirstOrDefaultAsync(element => element.ID == id);

            if(element == null)
            {
                return NotFound();
            }

            var elementList = element.List;
            if (elementList == null)
            {
                return NotFound();
            }

            try
            {
                if (image != null)
                    using (var ms = new MemoryStream())
                    {
                        image.CopyTo(ms);
                        element.Image = ms.ToArray();
                    }

                GetTags(tagsString, element, elementList);

                _context.Update(element);
                _context.Update(elementList);
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
            return RedirectToAction("Details", "ElementLists", new { id = listID });
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
                _context.RemoveElement(element);
            }

            await _context.SaveChangesAsync();
            if (element != null)
            {
                return RedirectToAction("Details", "ElementLists", new { id = element.List.ID });
            }
            else
                return RedirectToAction("Index", "ElementLists");
        }

        private bool ElementExists(int id)
        {
          return (_context.Elements?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        private void GetTags(string? tagString, Element element, ElementList elementList)
        {
            if (tagString != null)
            {
                string[] tags = tagString.Split(',');
                for(int i = 0; i < tags.Length; i++)
                {
                    tags[i] = tags[i].Trim();
                }

                if(element.Tags != null)
                {
                    IEnumerable<ElementTag> removeTags = element.Tags.Where(x => !tags.Any(y => x.Tag == y));
                    foreach(var tag in removeTags)
                    {
                        element.Tags.Remove(tag);
                        _context.VerifyTag(tag);
                    }
                }

                foreach (var tagName in tags.Where(x => !element.Tags?.Any(y => x == y.Tag) ?? true))
                {
                    ElementTag? tag = elementList.Tags.FirstOrDefault(x => x.Tag == tagName);
                    if (tag == default)
                    {
                        tag = new ElementTag() { Tag = tagName, Elements = new List<Element>() { element } };
                        elementList.Tags.Add(tag);
                    }
                    element.Tags!.Add(tag);
                }
            }
            else
            {
                if(element.Tags == null)
                {
                    element.Tags = new();
                }
                else
                {
                    List<ElementTag> copy = new(element.Tags);
                    element.Tags.Clear();
                    foreach(var tag in copy)
                    {
                        _context.VerifyTag(tag);
                    }       
                }
            }
        }
    }
}
