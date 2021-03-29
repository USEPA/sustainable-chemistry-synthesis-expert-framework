using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// See answer here for getting webroot...
// https://stackoverflow.com/questions/43709657/how-to-get-root-directory-of-project-in-asp-net-core-directory-getcurrentdirect

namespace SustainableChemistryWeb.Controllers
{
    public class NamedreactionsController : Controller
    {
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;
        private readonly SustainableChemistryContext _context;

        public NamedreactionsController(SustainableChemistryContext context, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Namedreactions
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]

        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            var reactions = await _context.AppNamedreaction
                .Include(s => s.FunctionalGroup)
                .ToListAsync();
            //reactions.sort();

            switch (sortOrder)
            {
                default:
                    reactions = reactions.OrderBy(s => s.FunctionalGroup.Name).ThenBy(s => s.Name).ToList();
                    break;
            }


            if (!String.IsNullOrEmpty(searchString))
            {
                reactions = reactions.Where(s => s.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) || s.FunctionalGroup.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            return View(reactions);
        }

        // GET: Namedreactions/Details/5
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appNamedreaction = await _context.AppNamedreaction
                .Include(a => a.Catalyst)
                .Include(a => a.FunctionalGroup)
                .Include(a => a.Solvent)
                .Include(a => a.AppNamedreactionReactants)
                    .ThenInclude(a => a.Reactant)
                .Include(a => a.AppNamedreactionByProducts)
                    .ThenInclude(a => a.Reactant)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appNamedreaction == null)
            {
                return NotFound();
            }

            var reactionViewModel = new SustainableChemistryWeb.ViewModels.NamedReactionViewModel
            {
                Id = appNamedreaction.Id,
                Name = appNamedreaction.Name,
                FunctionalGroup = appNamedreaction.FunctionalGroup,
                Catalyst = appNamedreaction.Catalyst,
                Solvent = appNamedreaction.Solvent,
                Product = appNamedreaction.Product,
                AppNamedreactionByProducts = appNamedreaction.AppNamedreactionByProducts,
                AppNamedreactionReactants = appNamedreaction.AppNamedreactionReactants,
                AppReference = appNamedreaction.AppReference,
                Url = appNamedreaction.Url,
                Image = appNamedreaction.Image,
            };

            if (appNamedreaction.Heat == "HE") reactionViewModel.Heat = "Heat";
            else reactionViewModel.Heat = "Not Applicable";

            if (appNamedreaction.AcidBase == "AC") reactionViewModel.AcidBase = "Acid";
            else if (appNamedreaction.AcidBase == "BA") reactionViewModel.AcidBase = "Base";
            else if (appNamedreaction.AcidBase == "AB") reactionViewModel.AcidBase = "Acid Or Base";
            else reactionViewModel.AcidBase = "Not Applicable";

            return View(reactionViewModel);
        }

        // GET: Namedreactions/Create
        public IActionResult Create()
        {
            var reaction = new NamedReaction
            {
                AppNamedreactionReactants = new List<NamedReactionReactants>(),
                AppNamedreactionByProducts = new List<NamedReactionByProducts>()
            };
            PopulateReactantData(reaction);
            List<SelectListItem> acidBaseList = new List<SelectListItem>
            {
                new SelectListItem { Value = "AC", Text = "Acid" },
                new SelectListItem { Value = "BA", Text = "Base" },
                new SelectListItem { Value = "AB", Text = "Acid Or Base" },
                new SelectListItem { Value = "NA", Text = "Not Applicable" }
            };
            ViewData["AcidBaseList"] = new SelectList(acidBaseList, "Value", "Text", "NA");
            List<SelectListItem> heatList = new List<SelectListItem>
            {
                new SelectListItem { Value = "HE", Text = "Heat" },
                new SelectListItem { Value = "NA", Text = "Not Applicable" }
            };
            ViewData["HeatList"] = new SelectList(heatList, "Value", "Text", "NA");
            ViewData["CatalystId"] = new SelectList(_context.AppCatalyst.OrderBy(i => i.Name.ToLower()).ToList(), "Id", "Name", 1);
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup.OrderBy(i => i.Name.ToLower()).ToList(), "Id", "Name");
            ViewData["SolventId"] = new SelectList(_context.AppSolvent.OrderBy(i => i.Name.ToLower()).ToList(), "Id", "Name", 1);
            return View();
        }

        // POST: Namedreactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Needs fixed!
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ReactantA,ReactantB,ReactantC,Product,Heat,AcidBase,ImageFile,CatalystId,FunctionalGroupId,SolventId,Url")] SustainableChemistryWeb.ViewModels.NamedReactionViewModel namedReactionView, string[] reactants, string[] byProducts)
        {
            NamedReaction appNamedreaction = new NamedReaction()
            {
                Name = namedReactionView.Name,
                ReactantA = string.Empty,
                ReactantB = string.Empty,
                ReactantC = string.Empty,
                Product = string.IsNullOrEmpty(namedReactionView.Product) ? string.Empty : namedReactionView.Product,
                Heat = namedReactionView.Heat,
                AcidBase = namedReactionView.AcidBase,
                //Image = "Images/Reactions/" + Guid.NewGuid().ToString() + namedReactionView.Image == null ? ".jpg" : ,
                CatalystId = namedReactionView.CatalystId,
                FunctionalGroupId = namedReactionView.FunctionalGroupId,
                SolventId = namedReactionView.SolventId,
                Url = string.IsNullOrEmpty(namedReactionView.Url) ? string.Empty : namedReactionView.Url,
            };
            if (namedReactionView.Image != null)
            {
                appNamedreaction.Image = "Images/Reactions/" + Guid.NewGuid().ToString() + System.IO.Path.GetFileName(namedReactionView.ImageFile.FileName);
                using (var stream = new System.IO.FileStream(_hostingEnvironment.WebRootPath + "/" + appNamedreaction.Image, System.IO.FileMode.Create))
                {
                    await namedReactionView.ImageFile.CopyToAsync(stream);
                    stream.Close();
                }
            }
            else
            {
                appNamedreaction.Image = "Images/Reactions/" + Guid.NewGuid().ToString() + ".jpg";
                System.IO.StreamReader image = new System.IO.StreamReader(_hostingEnvironment.WebRootPath + "/Images/Reactions/th.jpg");
                using (var stream = new System.IO.FileStream(_hostingEnvironment.WebRootPath + "/" + appNamedreaction.Image, System.IO.FileMode.Create))
                {
                    await image.BaseStream.CopyToAsync(stream);
                    stream.Close();
                }
            }
            if (reactants != null)
            {
                foreach (var reactant in reactants)
                {
                    var reactantToAdd = new NamedReactionReactants { NamedreactionId = appNamedreaction.Id, ReactantId = long.Parse(reactant) };
                    appNamedreaction.AppNamedreactionReactants.Add(reactantToAdd);
                }
            }

            if (byProducts != null)
            {
                foreach (var reactant in byProducts)
                {
                    var byProductToAdd = new NamedReactionByProducts { NamedreactionId = appNamedreaction.Id, ReactantId = long.Parse(reactant) };
                    appNamedreaction.AppNamedreactionByProducts.Add(byProductToAdd);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(appNamedreaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            PopulateReactantData(appNamedreaction);
            return View(appNamedreaction);
        }

        // GET: Namedreactions/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appNamedreaction = await _context.AppNamedreaction
                .Include(i => i.FunctionalGroup)
                .Include(i => i.AppNamedreactionReactants).ThenInclude(i => i.Reactant)
                .Include(i => i.AppNamedreactionByProducts).ThenInclude(i => i.Reactant)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);

            if (appNamedreaction == null)
            {
                return NotFound();
            }

            ViewModels.NamedReactionViewModel viewModel = new ViewModels.NamedReactionViewModel()
            {
                Name = appNamedreaction.Name,
                Product = appNamedreaction.Product,
                Heat = appNamedreaction.Heat,
                AcidBase = appNamedreaction.AcidBase,
                CatalystId = appNamedreaction.CatalystId,
                FunctionalGroupId = appNamedreaction.FunctionalGroupId,
                SolventId = appNamedreaction.SolventId,
                Url = appNamedreaction.Url,
                Image = appNamedreaction.Image
            };
            PopulateReactantData(appNamedreaction);
            List<SelectListItem> acidBaseList = new List<SelectListItem>
            {
                new SelectListItem { Value = "AC", Text = "Acid" },
                new SelectListItem { Value = "BA", Text = "Base" },
                new SelectListItem { Value = "AB", Text = "Acid Or Base" },
                new SelectListItem { Value = "NA", Text = "Not Applicable" }
            };
            ViewData["AcidBaseList"] = new SelectList(acidBaseList, "Value", "Text", appNamedreaction.AcidBase);
            List<SelectListItem> heatList = new List<SelectListItem>
            {
                new SelectListItem { Value = "HE", Text = "Heat" },
                new SelectListItem { Value = "NA", Text = "Not Applicable" }
            };
            ViewData["HeatList"] = new SelectList(heatList, "Value", "Text", appNamedreaction.Heat);
            ViewData["CatalystId"] = new SelectList(_context.AppCatalyst.OrderBy(i => i.Name.ToLower()).ToList(), "Id", "Name", appNamedreaction.CatalystId);
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup.OrderBy(i => i.Name.ToLower()).ToList(), "Id", "Name", appNamedreaction.FunctionalGroupId);
            ViewData["SolventId"] = new SelectList(_context.AppSolvent.OrderBy(i => i.Name.ToLower()).ToList(), "Id", "Name", appNamedreaction.SolventId);
            return View(viewModel);
        }

        // POST: Namedreactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,FunctionalGroupId, Product,Heat,AcidBase,ImageFile,Image,Catalyst,SolventId,Url")] ViewModels.NamedReactionViewModel appNamedreaction, string[] reactants, string[] byProducts, string catalyst, string solvent)
        {
            if (ModelState.IsValid)
                if (id != appNamedreaction.Id)
                {
                    return NotFound();
                }

            var reactionToUpdate = await _context.AppNamedreaction
                .Include(i => i.FunctionalGroup)
                .Include(i => i.AppNamedreactionReactants)
                    .ThenInclude(i => i.Reactant)
                .Include(i => i.AppNamedreactionByProducts)
                    .ThenInclude(i => i.Reactant)
                .SingleOrDefaultAsync(m => m.Id == id);

            if (await TryUpdateModelAsync<NamedReaction>(
                reactionToUpdate,
                "",
                r => r.Name, r => r.Product, r => r.Heat, r => r.AcidBase, r => r.FunctionalGroupId, r => r.CatalystId, r => r.SolventId, r => r.Url))
            {
                var fileName = _hostingEnvironment.WebRootPath + "/" + reactionToUpdate.Image;
                if (appNamedreaction.ImageFile != null)
                {
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }
                    reactionToUpdate.Image = "Images/Reactions/" + Guid.NewGuid().ToString() + System.IO.Path.GetFileName(appNamedreaction.ImageFile.FileName);
                    using (var stream = new System.IO.FileStream(_hostingEnvironment.WebRootPath + "/" + reactionToUpdate.Image, System.IO.FileMode.Create))
                    {
                        await appNamedreaction.ImageFile.CopyToAsync(stream);
                        stream.Close();
                    }
                }
                UpdateNamedReactionReactants(reactants, reactionToUpdate);
                UpdateNamedReactionByProducts(byProducts, reactionToUpdate);
                if (string.IsNullOrEmpty(reactionToUpdate.Url)) reactionToUpdate.Url = string.Empty;
                if (string.IsNullOrEmpty(reactionToUpdate.Product)) reactionToUpdate.Product = string.Empty;
                //try
                //{
                _context.SaveChanges();
                //}
                //catch (DbUpdateException /* ex */)
                //{
                //    //Log the error (uncomment ex variable name and write a log.)
                //    ModelState.AddModelError("", "Unable to save changes. " +
                //        "Try again, and if the problem persists, " +
                //        "see your system administrator.");
                //}
                return RedirectToAction(nameof(Index));
            }
            // PopulateReactantData(appNamedreaction);
            ViewData["CatalystId"] = new SelectList(_context.AppCatalyst.OrderBy(i => i.Name.ToLower()).ToList(), "Id", "Name", appNamedreaction.CatalystId);
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup.OrderBy(i => i.Name.ToLower()).ToList(), "Id", "Name", appNamedreaction.FunctionalGroupId);
            ViewData["SolventId"] = new SelectList(_context.AppSolvent.OrderBy(i => i.Name.ToLower()).ToList(), "Id", "Name", appNamedreaction.SolventId);
            return View(appNamedreaction);
        }

        private void PopulateReactantData(NamedReaction reaction)
        {
            var allReactants = _context.AppReactant.OrderBy(i => i.Name.ToLower()).ToList();
            var reactionReactants = new HashSet<long>(reaction.AppNamedreactionReactants.Select(c => c.ReactantId));
            var reactantList = new List<SelectListItem>();
            List<string> selectedReactants = new List<string>();
            var reactionByProducts = new HashSet<long>(reaction.AppNamedreactionByProducts.Select(c => c.ReactantId));
            var byProductList = new List<SelectListItem>();
            List<string> selectedByProducts = new List<string>();
            foreach (var reactant in allReactants)
            {
                bool isReactant = reactionReactants.Contains(reactant.Id);
                if (isReactant) selectedReactants.Add(reactant.Id.ToString());
                reactantList.Add(new SelectListItem
                {
                    Value = reactant.Id.ToString(),
                    Text = reactant.Name,
                    Selected = isReactant
                });
                bool isByProduct = reactionByProducts.Contains(reactant.Id);
                if (isByProduct) selectedByProducts.Add(reactant.Id.ToString());
                byProductList.Add(new SelectListItem
                {
                    Value = reactant.Id.ToString(),
                    Text = reactant.Name,
                    Selected = isByProduct
                });
            }
            ViewData["Reactants"] = new MultiSelectList(reactantList, "Value", "Text", selectedReactants);
            ViewData["ByProducts"] = new MultiSelectList(byProductList, "Value", "Text", selectedByProducts);
        }

        private void UpdateNamedReactionReactants(string[] selectedReactants, NamedReaction reactionToUpdate)
        {
            if (selectedReactants == null)
            {
                reactionToUpdate.AppNamedreactionReactants = new List<NamedReactionReactants>();
                return;
            }

            var selectedReactantsHS = new HashSet<string>(selectedReactants);
            var reactionReactants = new HashSet<long>
                (reactionToUpdate.AppNamedreactionReactants.Select(c => c.Reactant.Id));
            foreach (var reaction in _context.AppReactant)
            {
                if (selectedReactantsHS.Contains(reaction.Id.ToString()))
                {
                    if (!reactionReactants.Contains(reaction.Id))
                    {
                        reactionToUpdate.AppNamedreactionReactants.Add(new NamedReactionReactants { NamedreactionId = reactionToUpdate.Id, ReactantId = reaction.Id });
                    }
                }
                else
                {

                    if (reactionReactants.Contains(reaction.Id))
                    {
                        NamedReactionReactants reactionToRemove = reactionToUpdate.AppNamedreactionReactants.SingleOrDefault(i => i.ReactantId == reaction.Id);
                        _context.Remove(reactionToRemove);
                    }
                }
            }
        }

        private void UpdateNamedReactionByProducts(string[] selectedByProducts, NamedReaction reactionToUpdate)
        {
            if (selectedByProducts == null)
            {
                reactionToUpdate.AppNamedreactionByProducts = new List<NamedReactionByProducts>();
                return;
            }

            var selectedReactantsHS = new HashSet<string>(selectedByProducts);
            var reactionByProducts = new HashSet<long>
                (reactionToUpdate.AppNamedreactionByProducts.Select(c => c.Reactant.Id));
            foreach (var reactant in _context.AppReactant)
            {
                if (selectedReactantsHS.Contains(reactant.Id.ToString()))
                {
                    if (!reactionByProducts.Contains(reactant.Id))
                    {
                        reactionToUpdate.AppNamedreactionByProducts.Add(new NamedReactionByProducts { NamedreactionId = reactionToUpdate.Id, ReactantId = reactant.Id });
                    }
                }
                else
                {

                    if (reactionByProducts.Contains(reactant.Id))
                    {
                        NamedReactionByProducts courseToRemove = reactionToUpdate.AppNamedreactionByProducts.SingleOrDefault(i => i.ReactantId == reactant.Id);
                        _context.Remove(courseToRemove);
                    }
                }
            }
        }

        // GET: Namedreactions/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appNamedreaction = await _context.AppNamedreaction
                .Include(a => a.Catalyst)
                .Include(a => a.FunctionalGroup)
                .Include(a => a.Solvent)
                .Include(a => a.AppNamedreactionReactants)
                .Include(a => a.AppNamedreactionByProducts)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appNamedreaction == null)
            {
                return NotFound();
            }

            return View(appNamedreaction);
        }

        // POST: Namedreactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var appNamedreaction = await _context.AppNamedreaction
                .Include(a => a.Catalyst)
                .Include(a => a.FunctionalGroup)
                .Include(a => a.Solvent)
                    .Include(i => i.AppNamedreactionReactants)
                        .ThenInclude(i => i.Reactant)
                    .Include(i => i.AppNamedreactionByProducts)
                        .ThenInclude(i => i.Reactant)
                .Include(a => a.AppNamedreactionByProducts)
                .FirstOrDefaultAsync(m => m.Id == id);

            var fileName = _hostingEnvironment.WebRootPath + "/" + appNamedreaction.Image;
            if (System.IO.File.Exists(fileName))
            {
                System.IO.File.Delete(fileName);
            }
            var reactionByProducts = new HashSet<long>
                (appNamedreaction.AppNamedreactionByProducts.Select(c => c.Reactant.Id));
            foreach (var reactant in _context.AppReactant)
            {
                if (reactionByProducts.Contains(reactant.Id))
                {
                    NamedReactionByProducts reactantToRemove = appNamedreaction.AppNamedreactionByProducts.SingleOrDefault(i => i.ReactantId == reactant.Id);
                    _context.Remove(reactantToRemove);
                }
            }

            var reactionReactants = new HashSet<long>
                (appNamedreaction.AppNamedreactionReactants.Select(c => c.Reactant.Id));
            foreach (var reactant in _context.AppReactant)
            {
                if (reactionReactants.Contains(reactant.Id))
                {
                    NamedReactionReactants reactantToRemove = appNamedreaction.AppNamedreactionReactants.SingleOrDefault(i => i.ReactantId == reactant.Id);
                    _context.Remove(reactantToRemove);
                }
            }
            appNamedreaction.FunctionalGroup = null;
            appNamedreaction.Catalyst = null;
            appNamedreaction.Solvent = null;
            var references = _context.AppReference.Where(i => i.ReactionId == appNamedreaction.Id).ToList();
            foreach (Reference r in references) _context.AppReference.Remove(r);
            _context.AppNamedreaction.Remove(appNamedreaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppNamedreactionExists(long id)
        {
            return _context.AppNamedreaction.Any(e => e.Id == id);
        }
    }
}
