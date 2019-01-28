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
    public class AuthorController : Controller
    {
        // GET: Author
        public async Task<IActionResult> Index()
        {
            using (var context = new EFCoreWebDemoContext())
            {
                var model = await context.Authors.AsNoTracking().ToListAsync();
                return View(model);
            }

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName, LastName")] Author author)
        {
            using (var context = new EFCoreWebDemoContext())
            {
                if (ModelState.IsValid)
                {
                    context.Add(author);
                    await context.SaveChangesAsync();
                    //return RedirectToAction("Index");
                    //return View(author);
                }
                return RedirectToAction("Index");
            }
        }

        // GET: Author/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (var context = new EFCoreWebDemoContext())
            {
                //var author = await _context.Authors
                var author = await context.Authors
                   .FirstOrDefaultAsync(m => m.AuthorId == id);
                if (author == null)
                {
                    return NotFound();
                }
                return View(author);
            }
        }

        // GET: Author/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (var context = new EFCoreWebDemoContext())
            {
                var author = await context.Authors.FindAsync(id);
                if (author == null)
                {
                    return NotFound();
                }
                return View(author);
            }
        }
        // POST: Author/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AuthorId,FirstName,LastName")] Author author)
        {
            if (id != author.AuthorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var context = new EFCoreWebDemoContext())
                    {
                        context.Update(author);
                        await context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.AuthorId))
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
            //return View(author);
            return RedirectToAction("Index");
        }

        // GET: Author/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (var context = new EFCoreWebDemoContext())
            {
                var author = await context.Authors
                .FirstOrDefaultAsync(m => m.AuthorId == id);

                if (author == null)
                {
                    return NotFound();
                }
                return View(author);
                
            }
        }

        // POST: Author/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            using (var context = new EFCoreWebDemoContext())
            {
                var author = await context.Authors.FindAsync(id);
                context.Authors.Remove(author);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
        }

        private bool AuthorExists(int id)
        {
            using (var context = new EFCoreWebDemoContext())
            {
                return context.Authors.Any(e => e.AuthorId == id);
            }
        }
    }
}

