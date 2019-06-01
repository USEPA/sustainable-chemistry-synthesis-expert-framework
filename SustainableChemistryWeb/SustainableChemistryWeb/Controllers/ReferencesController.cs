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
    public class ReferencesController : Controller
    {
        private readonly SustainableChemistryContext _context;

        public ReferencesController(SustainableChemistryContext context)
        {
            _context = context;
        }

        // GET: AppReferences
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Index(string functionalGroupSearchString, int? namedReactionId)
        {
            var sustainableChemistryContext = _context.AppReference.Include(a => a.FunctionalGroup).Include(a => a.Reaction);
            var list = await sustainableChemistryContext.ToListAsync();
            if (!String.IsNullOrEmpty(functionalGroupSearchString))
            {
                list = list.Where(s => s.FunctionalGroup.Name.Contains(functionalGroupSearchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (namedReactionId != null)
            {
                list = list.Where(s => s.ReactionId == namedReactionId).ToList();
            }


            var referenceViewModels = new List<SustainableChemistryWeb.ViewModels.ReferenceViewModel>();
            foreach(var referecnce in list)
            {
                referenceViewModels.Add(new SustainableChemistryWeb.ViewModels.ReferenceViewModel
                {
                    Id = referecnce.Id,
                    FunctionalGroupId = referecnce.FunctionalGroupId,
                    FunctionalGroup = referecnce.FunctionalGroup,
                    ReactionId = referecnce.ReactionId,
                    Reaction = referecnce.Reaction,
                    Risdata = referecnce.Risdata
                });
            }
            return View(referenceViewModels);
        }

        // GET: AppReferences/Details/5
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
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

            var referenceViewModel = new SustainableChemistryWeb.ViewModels.ReferenceViewModel
            {
                Id = appReference.Id,
                FunctionalGroupId = appReference.FunctionalGroupId,
                FunctionalGroup = appReference.FunctionalGroup,
                ReactionId = appReference.ReactionId,
                Reaction = appReference.Reaction,
                Risdata = appReference.Risdata
            };

            return View(referenceViewModel);
        }

        // GET: AppReferences/Create
        public IActionResult Create()
        {
            PopulateReferenceData();
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Name");
            return View();
        }

        // POST: AppReferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Risdata,FunctionalGroupId,ReactionId")] Reference appReference)
        {
            if (System.IO.File.Exists(appReference.Risdata))
            {
                    appReference.Risdata = System.IO.File.ReadAllText(appReference.Risdata);
            }
            if (ModelState.IsValid)
            {
                _context.Add(appReference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateReferenceData();
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Name", appReference.FunctionalGroupId);
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
            var referenceViewModel = new SustainableChemistryWeb.ViewModels.ReferenceViewModel
            {
                Id = appReference.Id,
                FunctionalGroupId = appReference.FunctionalGroupId,
                FunctionalGroup = appReference.FunctionalGroup,
                ReactionId = appReference.ReactionId,
                Reaction = appReference.Reaction,
                Risdata = appReference.Risdata
            };
            PopulateReferenceData();
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Name", appReference.FunctionalGroupId);
            ViewData["ReferenceText"] = referenceViewModel.Text;
            return View(appReference);
        }

        // POST: AppReferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Risdata,FunctionalGroupId,ReactionId")] Reference appReference)
        {
            if (id != appReference.Id)
            {
                return NotFound();
            }

            if (!System.IO.File.Exists(appReference.Risdata))
            {
                return NotFound("RIS Data file does not exist");
            }

            appReference.Risdata = System.IO.File.ReadAllText(appReference.Risdata);

            var referenceViewModel = new SustainableChemistryWeb.ViewModels.ReferenceViewModel
            {
                Id = appReference.Id,
                FunctionalGroupId = appReference.FunctionalGroupId,
                FunctionalGroup = appReference.FunctionalGroup,
                ReactionId = appReference.ReactionId,
                Reaction = appReference.Reaction,
                Risdata = appReference.Risdata
            };

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
            PopulateReferenceData();
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Name", appReference.FunctionalGroupId);
            ViewData["ReferenceText"] = referenceViewModel.Text;
            return View(appReference);
        }

        private void PopulateReferenceData()
        {
            ViewData["ReactionId"] = new SelectList(_context.AppNamedreaction.Include(a => a.FunctionalGroup), "Id", "Name", "1", "FunctionalGroup.Name");
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

            var referenceViewModel = new SustainableChemistryWeb.ViewModels.ReferenceViewModel
            {
                Id = appReference.Id,
                FunctionalGroupId = appReference.FunctionalGroupId,
                FunctionalGroup = appReference.FunctionalGroup,
                ReactionId = appReference.ReactionId,
                Reaction = appReference.Reaction,
                Risdata = appReference.Risdata
            };
            ViewData["ReferenceText"] = referenceViewModel.Text;
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
