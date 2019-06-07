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
            foreach (var referecnce in list)
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
            PopulateReferenceData(null);
            return View();
        }

        // POST: AppReferences/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RISFile,FunctionalGroupId,ReactionId")] SustainableChemistryWeb.ViewModels.ReferenceViewModel appReference)
        {
            if (ModelState.IsValid)
            {
                if (appReference.RISFile != null)
                {
                    appReference.Risdata = new System.IO.StreamReader(appReference.RISFile.OpenReadStream()).ReadToEnd();
                }
                SustainableChemistryWeb.Models.Reference reference = new Reference
                {
                    FunctionalGroupId = appReference.FunctionalGroupId,
                    FunctionalGroup = appReference.FunctionalGroup,
                    ReactionId = appReference.ReactionId,
                    Reaction = appReference.Reaction,
                    Risdata = appReference.Risdata
                };
                _context.Add(reference);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateReferenceData(null);
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
            PopulateReferenceData(appReference);
            ViewData["ReferenceText"] = referenceViewModel.Text;
            return View(referenceViewModel);
        }

        // POST: AppReferences/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Risdata,FunctionalGroupId,ReactionId")] SustainableChemistryWeb.ViewModels.ReferenceViewModel appReference)
        {
            if (id != appReference.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var referenceToUpdate = await _context.AppReference
                    .SingleOrDefaultAsync(m => m.Id == id);

                if (await TryUpdateModelAsync<Reference>(
                    referenceToUpdate,
                    "",
                    r => r.FunctionalGroupId, r => r.ReactionId))
                    if (appReference.RISFile != null)
                    {
                        referenceToUpdate.Risdata = new System.IO.StreamReader(appReference.RISFile.OpenReadStream()).ReadToEnd();
                    }
                _context.Update(referenceToUpdate);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(id));
        }

        private void PopulateReferenceData(Reference appReference)
        {
            ViewData["Reaction"] = new SelectList(_context.AppNamedreaction.Include(a => a.FunctionalGroup), "Id", "Name", "1", "FunctionalGroup.Name");
            if (appReference != null) ViewData["FunctionalGroup"] = new SelectList(_context.AppFunctionalgroup, "Id", "Name", appReference.FunctionalGroupId);
            else ViewData["FunctionalGroup"] = new SelectList(_context.AppFunctionalgroup, "Id", "Name");
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
