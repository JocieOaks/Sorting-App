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
    /// <summary>
    /// The <see cref="ElementsController"/> class is an MVC <see cref="Controller"/> used to manipulate the <see cref="Element"/> model.
    /// </summary>
    public class ElementsController : Controller
    {
        private readonly SortingContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElementsController"/> class.
        /// </summary>
        /// <param name="context">The <see cref="SortingContext"/> used to query the database.</param>
        public ElementsController(SortingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gives the MVC View for creating a new <see cref="Element"/>.
        /// </summary>
        /// <param name="listID">The ID number associated with the <see cref="ElementList"/> to which the newly created <see cref="Element"/> will be added.</param>
        /// <returns>Returns the next View to show the user.</returns>
        public async Task<IActionResult> Create(int? listID)
        {
            if (listID == null || _context.ElementLists == null)
                return NotFound();

            var elementList = await _context.ElementLists
                .FirstOrDefaultAsync(m => m.ID == listID);
            if (elementList == null)
            {
                return NotFound();
            }
            return View(new Element() { List = elementList });
        }

        /// <summary>
        /// Creates a new <see cref="Element"/> based on input from the user.
        /// </summary>
        /// <param name="element">The newly created <see cref="Element"/> with it's <see cref="Element.Name"/> and <see cref="ElementList"/> bound.</param>
        /// <param name="image">The image associated with the <see cref="Element"/>, if one is given.</param>
        /// <param name="tagsString">The string that contains a comma seperated list of the <see cref="ElementTag"/>s for this element.
        /// This was made as a separate parameter instead of being bound to <c>element</c> because the <see cref="ElementTag"/> class
        /// contains a reference to the <see cref="Element"/>s it's on.
        /// However, this may be unnecessary, as that is rarely used.</param>
        /// <param name="listID">The ID number associated with the <see cref="ElementList"/> to which the newly created <see cref="Element"/> will be added.</param>
        /// <returns>Returns the next View to show the user.</returns>
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
                if (image != null)
                    using (var ms = new MemoryStream())
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
                return RedirectToAction("Details", "ElementLists", new { id = listID });
            }
            return View(element);
        }

        /// <summary>
        /// Gives the MVC View for deleting an <see cref="Element"/>.
        /// </summary>
        /// <param name="id">The ID number associated with the <see cref="Element"/> to be deleted.</param>
        /// <returns>Returns the next View to show the user.</returns>
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

        /// <summary>
        /// Deletes the given <see cref="Element"/> from the database.
        /// </summary>
        /// <param name="id">The ID number associated with the deleted <see cref="Element"/>.</param>
        /// <returns>Returns the next View to show the user.</returns>
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


        /// <summary>
        /// Gives the MVC View for showing the details of an <see cref="Element"/>.
        /// </summary>
        /// <param name="id">The ID number associated with the <see cref="Element"/>.</param>
        /// <returns>Returns the next View to show the user.</returns>
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

        /// <summary>
        /// Gives the MVC View for editing the details of an <see cref="Element"/>.
        /// </summary>
        /// <param name="id">The ID associated with the <see cref="Element"/> to be edited.</param>
        /// <returns>Returns the next View to show the user.</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Elements == null)
            {
                return NotFound();
            }

            var element = await _context.Elements.Include(element => element.Tags).FirstOrDefaultAsync(m => m.ID == id);
            if (element == null)
            {
                return NotFound();
            }
            return View(element);
        }

        /// <summary>
        /// Edits an <see cref="Element"/> based on input from the user.
        /// </summary>
        /// <param name="id">The ID number associated with the <see cref="Element"/> being edited.</param>
        /// <param name="Name">The name for the <see cref="Element"/>.</param>
        /// <param name="image">The new image associated with the <see cref="Element"/>, if one is given.</param>
        /// <param name="tagsString">The string that contains a comma seperated list of the <see cref="ElementTag"/>s for this element.
        /// This was made as a separate parameter instead of being bound to <c>element</c> because the <see cref="ElementTag"/> class
        /// contains a reference to the <see cref="Element"/>s it's on.
        /// However, this may be unnecessary, as that is rarely used.</param>
        /// <param name="listID">The ID number associated with the <see cref="ElementList"/>.</param>
        /// <returns>Returns the next View to show the user.</returns>
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
                element.Name = Name;
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

        /// <summary>
        /// Determines if an <see cref="Element"/> with the given ID is in the database.
        /// </summary>
        /// <param name="id">The ID number associated with the <see cref="Element"/> being searched for.</param>
        /// <returns>Returns true if the <see cref="Element"/> is found.</returns>
        private bool ElementExists(int id)
        {
          return (_context.Elements?.Any(e => e.ID == id)).GetValueOrDefault();
        }

        /// <summary>
        /// Takes a string designating the list of <see cref="ElementTag"/>s and adds them to a given <see cref="Element"/>.
        /// </summary>
        /// <param name="tagString">The list of <see cref="ElementTag"/>s as a comma separated string.</param>
        /// <param name="element">The <see cref="Element"/> with the <see cref="ElementTag"/>s.</param>
        /// <param name="elementList">The <see cref="ElementList"/> to which both the <see cref="Element"/> and <see cref="ElementTag"/>s belong.</param>
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
                    IEnumerable<ElementTag> removeTags = element.Tags.Where(x => !tags.Any(y => x.Tag == y)).ToList();
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
