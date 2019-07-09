using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;
using SustainableChemistryWeb.ViewModels;
using Microsoft.AspNetCore.Hosting;

// See answer here for getting webroot...
// https://stackoverflow.com/questions/43709657/how-to-get-root-directory-of-project-in-asp-net-core-directory-getcurrentdirect

namespace SustainableChemistryWeb.Controllers
{
    public class FunctionalGroupsController : Controller
    {
        private readonly SustainableChemistryContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FunctionalGroupsController(SustainableChemistryContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // O=P(OC)(OC)C
        // GET: FunctionalGroups
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Index(int? Id, int? funcGroupId, int? namedReactionId, string nameSearchString, string smilesSearchString)
        {
            var viewModel = new SustainableChemistryWeb.ViewModels.FunctionalGroupIndexData();
            viewModel.FunctionalGroups = await _context.AppFunctionalgroup
                  .Include(i => i.AppNamedreaction)
                    .ThenInclude(i => i.AppReference)
                  .AsNoTracking()
                  .OrderBy(i => i.Name)
                  .ToListAsync();

            List<FunctionalGroup> fgFound = new List<FunctionalGroup>();
            if (!String.IsNullOrEmpty(nameSearchString))
            {
                ViewData["SearchString"] = nameSearchString.Trim();
                fgFound.AddRange(viewModel.FunctionalGroups.Where(s => s.Name.Contains(nameSearchString, StringComparison.OrdinalIgnoreCase)));
            }

            else if (!String.IsNullOrEmpty(smilesSearchString))
            {
                ViewData["SmilesString"] = smilesSearchString;
                ChemInfo.Molecule molecule = new ChemInfo.Molecule(smilesSearchString.Trim());
                foreach (var fg in viewModel.FunctionalGroups)
                {
                    string smarts = fg.Smarts;
                    if (!string.IsNullOrEmpty(fg.Smarts))
                        if (molecule.FindFunctionalGroup(fg))
                        {
                            fgFound.Add(fg);
                        }

                }
                if (molecule.Aromatic) fgFound.Add(_context.AppFunctionalgroup
                    .FirstOrDefault(m => m.Id == 35));
                if (molecule.Heterocyclic) fgFound.Add(_context.AppFunctionalgroup
                    .FirstOrDefault(m => m.Id == 118));
                if (molecule.HeterocyclicAromatic) fgFound.Add(_context.AppFunctionalgroup
                    .FirstOrDefault(m => m.Id == 224));
            }

            else if (funcGroupId != null)
            {
                ViewData["FunctionalGroupID"] = funcGroupId.Value;
                FunctionalGroup group = viewModel.FunctionalGroups.Where(
                    i => i.Id == funcGroupId.Value).Single();
                fgFound.Add(group);
            }

            if (funcGroupId != null)
            {
                ViewData["FunctionalGroupID"] = funcGroupId.Value;
                FunctionalGroup group = viewModel.FunctionalGroups.Where(
                    i => i.Id == funcGroupId.Value).Single();
                viewModel.NamedReactions = group.AppNamedreaction;
                ViewData["FunctionalGroupName"] = group.Name;
            }

            if (namedReactionId != null)
            {
                ViewData["NamedReactionID"] = namedReactionId.Value;
                NamedReaction rxn = viewModel.NamedReactions.Where(
                    i => i.Id == namedReactionId.Value).Single();
                var referenceViewModels = new List<SustainableChemistryWeb.ViewModels.ReferenceViewModel>();
                ViewData["NamedReactionName"] = rxn.Name;
                foreach (var referecnce in rxn.AppReference)
                {
                    referenceViewModels.Add(new SustainableChemistryWeb.ViewModels.ReferenceViewModel
                    {
                        Id = referecnce.Id,
                        FunctionalGroupId = referecnce.FunctionalGroupId,
                        FunctionalGroup = viewModel.FunctionalGroups.Where(
                            i => i.Id == referecnce.FunctionalGroupId).Single(),
                        ReactionId = referecnce.ReactionId,
                        Reaction = referecnce.Reaction,
                        Risdata = referecnce.Risdata
                    });
                }
                viewModel.References = referenceViewModels;
            }
            if (fgFound.Count > 0) viewModel.FunctionalGroups = fgFound.OrderBy(i => i.Name);
            return View(viewModel);
        }

        // GET: FunctionalGroups/Details/5
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appFunctionalgroup = await _context.AppFunctionalgroup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appFunctionalgroup == null)
            {
                return NotFound();
            }

            return View(appFunctionalgroup);
        }

        // GET: FunctionalGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FunctionalGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Smarts,ImageFile,URL")] SustainableChemistryWeb.ViewModels.FunctionalGroupViewModel functionalGroupView)
        {
            if (ModelState.IsValid)
            {
                string name = System.IO.Path.GetFileName(functionalGroupView.ImageFile.FileName);
                FunctionalGroup appFunctionalGroup = new FunctionalGroup()
                {
                    Name = functionalGroupView.Name,
                    Smarts = functionalGroupView.Smarts,
                    Image = "Images/FunctionalGroups/" + name,
                    URL = functionalGroupView.URL
                };
                if (functionalGroupView.ImageFile.Length > 0)
                {
                    using (var stream = new System.IO.FileStream(_hostingEnvironment.WebRootPath + "/Images/FunctionalGroups/" + name, System.IO.FileMode.Create))
                    {
                        await functionalGroupView.ImageFile.CopyToAsync(stream);
                        stream.Close();
                    }
                }
                _context.Add(appFunctionalGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(functionalGroupView);
        }

        // GET: FunctionalGroups/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appFunctionalgroup = await _context.AppFunctionalgroup.FindAsync(id);
            if (appFunctionalgroup == null)
            {
                return NotFound();
            }
            SustainableChemistryWeb.ViewModels.FunctionalGroupViewModel functionalGroupView = new SustainableChemistryWeb.ViewModels.FunctionalGroupViewModel()
            {
                Name = appFunctionalgroup.Name,
                Smarts = appFunctionalgroup.Smarts,
                Image = appFunctionalgroup.Image,
                URL = appFunctionalgroup.URL
            };

            return View(functionalGroupView);
        }

        // POST: FunctionalGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Smarts,ImageFile,URL")] SustainableChemistryWeb.ViewModels.FunctionalGroupViewModel functionalGroupView)
        {
            if (id != functionalGroupView.Id)
            {
                return NotFound();
            }

            var functionalGroupToUpdate = await _context.AppFunctionalgroup
                .SingleOrDefaultAsync(m => m.Id == id);

            if (await TryUpdateModelAsync<FunctionalGroup>(
                functionalGroupToUpdate,
                "",
                r => r.Name, r => r.Smarts, r => r.Smarts, r => r.URL))
            {
                try
                {
                    var fileName = _hostingEnvironment.WebRootPath + "/" + functionalGroupToUpdate.Image;
                    if (functionalGroupView.ImageFile != null)
                    {
                        if (System.IO.File.Exists(fileName))
                        {
                            System.IO.File.Delete(fileName);
                        }
                        functionalGroupToUpdate.Image = "Images/FunctionalGroups/" + Guid.NewGuid().ToString() + System.IO.Path.GetFileName(functionalGroupView.ImageFile.FileName);
                        using (var stream = new System.IO.FileStream(_hostingEnvironment.WebRootPath + "/" + functionalGroupToUpdate.Image, System.IO.FileMode.Create))
                        {
                            await functionalGroupView.ImageFile.CopyToAsync(stream);
                            stream.Close();
                        }
                    }
                    //else
                    //{
                    //    functionalGroupToUpdate.Image = "Images/FunctionalGroups/" + Guid.NewGuid().ToString() + ".jpg";
                    //    System.IO.StreamReader image = new System.IO.StreamReader(_hostingEnvironment.WebRootPath + "/Images/Reactions/th.jpg");
                    //    using (var stream = new System.IO.FileStream(_hostingEnvironment.WebRootPath + "/" + functionalGroupToUpdate.Image, System.IO.FileMode.Create))
                    //    {
                    //        await image.BaseStream.CopyToAsync(stream);
                    //        stream.Close();
                    //    }
                    //}
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppFunctionalgroupExists(functionalGroupToUpdate.Id))
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
            return View(functionalGroupView);
        }

        // GET: FunctionalGroups/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appFunctionalgroup = await _context.AppFunctionalgroup
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appFunctionalgroup == null)
            {
                return NotFound();
            }

            return View(appFunctionalgroup);
        }

        // POST: FunctionalGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var appFunctionalgroup = await _context.AppFunctionalgroup.FindAsync(id);
            var fileName = _hostingEnvironment.WebRootPath + "/" + appFunctionalgroup.Image;
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
            var reactions = _context.AppNamedreaction
                .Include(a => a.Catalyst)
                .Include(a => a.FunctionalGroup)
                .Include(a => a.Solvent)
                    .Include(i => i.AppNamedreactionReactants)
                        .ThenInclude(i => i.Reactant)
                    .Include(i => i.AppNamedreactionByProducts)
                        .ThenInclude(i => i.Reactant)
                .Include(a => a.AppNamedreactionByProducts)
                .Where(i => i.FunctionalGroupId == id).ToList();
            foreach (NamedReaction rxn in reactions)
            {
                fileName = _hostingEnvironment.WebRootPath + "/" + rxn.Image;
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
                var reactionByProducts = new HashSet<long>
                    (rxn.AppNamedreactionByProducts.Select(c => c.Reactant.Id));
                foreach (var reactant in _context.AppReactant)
                {
                    if (reactionByProducts.Contains(reactant.Id))
                    {
                        NamedReactionByProducts reactantToRemove = rxn.AppNamedreactionByProducts.SingleOrDefault(i => i.ReactantId == reactant.Id);
                        _context.Remove(reactantToRemove);
                    }
                }

                var reactionReactants = new HashSet<long>
                    (rxn.AppNamedreactionReactants.Select(c => c.Reactant.Id));
                foreach (var reactant in _context.AppReactant)
                {
                    if (reactionReactants.Contains(reactant.Id))
                    {
                        NamedReactionReactants reactantToRemove = rxn.AppNamedreactionReactants.SingleOrDefault(i => i.ReactantId == reactant.Id);
                        _context.Remove(reactantToRemove);
                    }
                }
                rxn.FunctionalGroup = null;
                rxn.Catalyst = null;
                rxn.Solvent = null;
                _context.AppNamedreaction.Remove(rxn);
            }
            var references = _context.AppReference.Where(i => i.FunctionalGroupId == id).ToList();
            foreach (Reference r in references) _context.AppReference.Remove(r);

            _context.AppFunctionalgroup.Remove(appFunctionalgroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppFunctionalgroupExists(long id)
        {
            return _context.AppFunctionalgroup.Any(e => e.Id == id);
        }
    }
}
