using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EFMVCCoreSampleApp.Models;

namespace EFMVCCoreSampleApp.Controllers
{
    public class BooksController : Controller
    {
        public async Task<IActionResult> Index()
        {
            //var context = new EFCoreWebDemoContext();
            using (var context = new EFCoreWebDemoContext())
            {
                var model = await context.Books.Include(a => a.Author).AsNoTracking().ToListAsync();
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            using (var _context = new EFCoreWebDemoContext())
            {
                var authors = await _context.Authors.Select(a => new SelectListItem
                {
                    Value = a.AuthorId.ToString(),
                    Text = $"{a.FirstName} {a.LastName}"
                }).ToListAsync();
                ViewBag.Authors = authors;

                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title, AuthorId")] Book book)
        {
            if (ModelState.IsValid)
            {
                using (var _context = new EFCoreWebDemoContext())
                {
                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            using (var _context = new EFCoreWebDemoContext())
            {
                ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", book.AuthorId);
            }
            return View(book);
            //using (var context = new EFCoreWebDemoContext())
            //{
            //    context.Books.Add(book);
            //    await context.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (var _context = new EFCoreWebDemoContext())
            {
                var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.BookId == id);

                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }
        }
        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (var _context = new EFCoreWebDemoContext())
            {
                var book = await _context.Books.FindAsync(id);
                if (book == null)
                {
                    return NotFound();
                }
                var authors = await _context.Authors.Select(a => new SelectListItem
                {
                    Value = a.AuthorId.ToString(),
                    Text = $"{a.FirstName} {a.LastName}"
                }).ToListAsync();
                ViewBag.Authors = authors;
                //ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", book.AuthorId);
                return View(book);
            }
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,AuthorId")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var _context = new EFCoreWebDemoContext())
                    {
                        _context.Update(book);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
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
            using (var _context = new EFCoreWebDemoContext())
            {
                ViewData["AuthorId"] = new SelectList(_context.Authors, "AuthorId", "AuthorId", book.AuthorId);
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (var _context = new EFCoreWebDemoContext())
            {
                var book = await _context.Books
                .Include(b => b.Author)
                .FirstOrDefaultAsync(m => m.BookId == id);
                if (book == null)
                {
                    return NotFound();
                }
                return View(book);
            }
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var _context = new EFCoreWebDemoContext())
            {
                var book = await _context.Books.FindAsync(id);
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        private bool BookExists(int id)
        {
            using (var _context = new EFCoreWebDemoContext())
            {
                return _context.Books.Any(e => e.BookId == id);
            }
        }
    }
}
