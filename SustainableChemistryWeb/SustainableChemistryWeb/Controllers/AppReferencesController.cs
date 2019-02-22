using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;

namespace SustainableChemistryWeb.Controllers
{
    public class AppReferencesController : Controller
    {
        private readonly SustainableChemistryContext _context;

        public AppReferencesController(SustainableChemistryContext context)
        {
            _context = context;
        }

        // GET: AppReferences
        public async Task<IActionResult> Index()
        {
            var sustainableChemistryContext = _context.AppReference.Include(a => a.FunctionalGroup).Include(a => a.Reaction);
            return View(await sustainableChemistryContext.ToListAsync());
        }

        // GET: AppReferences/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appReference = await _context.AppReference
                .Include(a => a.FunctionalGroup)
                .Include(a => a.Reaction)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appReference == null)
            {
                return NotFound();
            }

            return View(appReference);
        }

        // GET: AppReferences/Create
        public IActionResult Create()
        {
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Image");
            ViewData["ReactionId"] = new SelectList(_context.AppNamedreaction, "Id", "AcidBase");
            return View();
        }

        // POST: AppReferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Risdata,FunctionalGroupId,ReactionId")] AppReference appReference)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appReference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Image", appReference.FunctionalGroupId);
            ViewData["ReactionId"] = new SelectList(_context.AppNamedreaction, "Id", "AcidBase", appReference.ReactionId);
            return View(appReference);
        }

        // GET: AppReferences/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appReference = await _context.AppReference.FindAsync(id);
            if (appReference == null)
            {
                return NotFound();
            }
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Image", appReference.FunctionalGroupId);
            ViewData["ReactionId"] = new SelectList(_context.AppNamedreaction, "Id", "AcidBase", appReference.ReactionId);
            return View(appReference);
        }

        // POST: AppReferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Risdata,FunctionalGroupId,ReactionId")] AppReference appReference)
        {
            if (id != appReference.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appReference);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppReferenceExists(appReference.Id))
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
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Image", appReference.FunctionalGroupId);
            ViewData["ReactionId"] = new SelectList(_context.AppNamedreaction, "Id", "AcidBase", appReference.ReactionId);
            return View(appReference);
        }

        // GET: AppReferences/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appReference = await _context.AppReference
                .Include(a => a.FunctionalGroup)
                .Include(a => a.Reaction)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appReference == null)
            {
                return NotFound();
            }

            return View(appReference);
        }

        // POST: AppReferences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var appReference = await _context.AppReference.FindAsync(id);
            _context.AppReference.Remove(appReference);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppReferenceExists(long id)
        {
            return _context.AppReference.Any(e => e.Id == id);
        }
    }
}
