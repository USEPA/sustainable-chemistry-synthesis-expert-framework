using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;
using Microsoft.AspNetCore.Hosting;

// See answer here for getting webroot...
// https://stackoverflow.com/questions/43709657/how-to-get-root-directory-of-project-in-asp-net-core-directory-getcurrentdirect

namespace SustainableChemistryWeb.Controllers
{
    public class NamedreactionsController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly SustainableChemistryContext _context;

        public NamedreactionsController(SustainableChemistryContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Namedreactions
        public async Task<IActionResult> Index(string nameSearchString, string funcGroupSearchString)
        {
            var reactions = from s in _context.AppNamedreaction
                           select s;

            if (!String.IsNullOrEmpty(nameSearchString))
            {
                reactions = reactions.Where(s => s.Name.Contains(nameSearchString, StringComparison.OrdinalIgnoreCase));
            }

            if (!String.IsNullOrEmpty(funcGroupSearchString))
            {
                reactions = reactions.Where(s => s.Name.Contains(funcGroupSearchString, StringComparison.OrdinalIgnoreCase));
            }

            var sustainableChemistryContext = reactions.Include(a => a.Catalyst).Include(a => a.FunctionalGroup).Include(a => a.Solvent);
            return View(await sustainableChemistryContext.AsNoTracking().ToListAsync());
        }

        // GET: Namedreactions/Details/5
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
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appNamedreaction == null)
            {
                return NotFound();
            }

            return View(appNamedreaction);
        }

        // GET: Namedreactions/Create
        public IActionResult Create()
        {
            List<SelectListItem> acidBaseList = new List<SelectListItem>
            {
                new SelectListItem { Value = "AC", Text = "Acid" },
                new SelectListItem { Value = "BA", Text = "Base" },
                new SelectListItem { Value = "AB", Text = "Acid Or Base" },
                new SelectListItem { Value = "NA", Text = "Not Applicable" }
            };
            ViewData["AcidBaseList"] = new SelectList(acidBaseList, "Value", "Text");
            List<SelectListItem> heatList = new List<SelectListItem>
            {
                new SelectListItem { Value = "HE", Text = "Heat" },
                new SelectListItem { Value = "NA", Text = "Not Applicable" }
            };
            ViewData["HeatList"] = new SelectList(heatList, "Value", "Text");
            ViewData["CatalystId"] = new SelectList(_context.AppCatalyst, "Id", "Name");
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Name");
            ViewData["SolventId"] = new SelectList(_context.AppSolvent, "Id", "Name");
            return View();
        }

        // POST: Namedreactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ReactantA,ReactantB,ReactantC,Product,Heat,AcidBase,Image,CatalystId,FunctionalGroupId,SolventId,Url")] AppNamedreaction appNamedreaction, string[] reactants, string[] byProducts)
        {
            if (appNamedreaction.Image != null && appNamedreaction.Image.Length > 0)
            {
                var file = System.IO.File.Open(appNamedreaction.Image, System.IO.FileMode.Open);
                appNamedreaction.Image = "Images/Reactions/" + System.IO.Path.GetFileName(file.Name);
                //There is an error here
                //var uploads = System.IO.Path.Combine(_appEnvironment.WebRootPath, "uploads\\img");
                if (file.Length > 0)
                {
                    var fileName = _hostingEnvironment.WebRootPath + "\\Images\\Reactions\\" + System.IO.Path.GetFileName(file.Name);
                    using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                file.Close();
            }

            if (ModelState.IsValid)
            {
                _context.Add(appNamedreaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            ViewData["Reactants"] = new MultiSelectList(_context.AppReactant, "Id", "Name", appNamedreaction.AppNamedreactionReactants);
            ViewData["CatalystId"] = new SelectList(_context.AppCatalyst, "Id", "Name", appNamedreaction.CatalystId);
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Name", appNamedreaction.FunctionalGroupId);
            ViewData["SolventId"] = new SelectList(_context.AppSolvent, "Id", "Name", appNamedreaction.SolventId);
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
                .Include(i => i.AppNamedreactionReactants).ThenInclude(i => i.Reactant)
                .Include(i => i.AppNamedreactionByProducts).ThenInclude(i => i.Reactant)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Id == id);

            if (appNamedreaction == null)
            {
                return NotFound();
            }
            PopulateReactantList(appNamedreaction);
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
            ViewData["CatalystId"] = new SelectList(_context.AppCatalyst, "Id", "Name", appNamedreaction.CatalystId);
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Name", appNamedreaction.FunctionalGroupId);
            ViewData["SolventId"] = new SelectList(_context.AppSolvent, "Id", "Name", appNamedreaction.SolventId);
            return View(appNamedreaction);
        }

        // POST: Namedreactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,ReactantA,ReactantB,ReactantC,Product,Heat,AcidBase,Image,CatalystId,FunctionalGroupId,SolventId,Url")] AppNamedreaction appNamedreaction, string[] reactants, string[] byProducts)
        {
            if (id != appNamedreaction.Id)
            {
                return NotFound();
            }

            if (appNamedreaction.Image != null && appNamedreaction.Image.Length > 0)
            {
                var file = System.IO.File.Open(appNamedreaction.Image, System.IO.FileMode.Open);
                appNamedreaction.Image = "Images/Reactions/" + System.IO.Path.GetFileName(file.Name);
                //There is an error here
                //var uploads = System.IO.Path.Combine(_appEnvironment.WebRootPath, "uploads\\img");
                if (file.Length > 0)
                {
                    var fileName = _hostingEnvironment.WebRootPath + "\\Images\\Reactions\\" + System.IO.Path.GetFileName(file.Name);
                    using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                file.Close();
            }
            var instructorToUpdate = await _context.AppNamedreaction
                    .Include(i => i.AppNamedreactionReactants)
                        .ThenInclude(i => i.Reactant)
                    .Include(i => i.AppNamedreactionByProducts)
                        .ThenInclude(i => i.Reactant)
                    .SingleOrDefaultAsync(m => m.Id == id);

            if (await TryUpdateModelAsync<AppNamedreaction>(
                instructorToUpdate,
                "", 
                r => r.Name, r => r.ReactantA, r => r.ReactantB, r => r.ReactantC, r => r.Product, r => r.Heat, r => r.AcidBase, r => r.CatalystId, r => r.FunctionalGroupId, r => r.SolventId, r => r.Url))
            {
                UpdateNamedReactionReactants(reactants, instructorToUpdate);
                UpdateNamedReactionByProducts(byProducts, instructorToUpdate);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
                return RedirectToAction(nameof(Index));
            }
            PopulateReactantData(appNamedreaction);
            ViewData["CatalystId"] = new SelectList(_context.AppCatalyst, "Id", "Name", appNamedreaction.CatalystId);
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Name", appNamedreaction.FunctionalGroupId);
            ViewData["SolventId"] = new SelectList(_context.AppSolvent, "Id", "Name", appNamedreaction.SolventId);
            return View(appNamedreaction);
        }

        private void PopulateReactantData(AppNamedreaction reaction)
        {
            var allReactants = _context.AppReactant;
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
            ViewData["Reactants"] = new MultiSelectList(reactantList, "Value","Text", selectedReactants);
            ViewData["ByProducts"] = new MultiSelectList(byProductList, "Value", "Text", selectedByProducts);
        }

        private void UpdateNamedReactionReactants(string[] selectedReactants, AppNamedreaction reactionToUpdate)
        {
            if (selectedReactants == null)
            {
                reactionToUpdate.AppNamedreactionReactants = new List<AppNamedreactionReactants>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedReactants);
            var instructorCourses = new HashSet<long>
                (reactionToUpdate.AppNamedreactionReactants.Select(c => c.Reactant.Id));
            foreach (var reaction in _context.AppReactant)
            {
                if (selectedCoursesHS.Contains(reaction.Id.ToString()))
                {
                    if (!instructorCourses.Contains(reaction.Id))
                    {
                        reactionToUpdate.AppNamedreactionReactants.Add(new AppNamedreactionReactants { NamedreactionId = reactionToUpdate.Id, ReactantId = reaction.Id });
                    }
                }
                else
                {

                    if (instructorCourses.Contains(reaction.Id))
                    {
                        AppNamedreactionReactants courseToRemove = reactionToUpdate.AppNamedreactionReactants.SingleOrDefault(i => i.ReactantId == reaction.Id);
                        _context.Remove(courseToRemove);
                    }
                }
            }
        }

        private void UpdateNamedReactionByProducts(string[] selectedByProducts, AppNamedreaction reactionToUpdate)
        {
            if (selectedByProducts == null)
            {
                reactionToUpdate.AppNamedreactionByProducts = new List<AppNamedreactionByProducts>();
                return;
            }

            var selectedCoursesHS = new HashSet<string>(selectedByProducts);
            var instructorCourses = new HashSet<long>
                (reactionToUpdate.AppNamedreactionByProducts.Select(c => c.Reactant.Id));
            foreach (var reaction in _context.AppReactant)
            {
                if (selectedCoursesHS.Contains(reaction.Id.ToString()))
                {
                    if (!instructorCourses.Contains(reaction.Id))
                    {
                        reactionToUpdate.AppNamedreactionByProducts.Add(new AppNamedreactionByProducts { NamedreactionId = reactionToUpdate.Id, ReactantId = reaction.Id });
                    }
                }
                else
                {

                    if (instructorCourses.Contains(reaction.Id))
                    {
                        AppNamedreactionByProducts courseToRemove = reactionToUpdate.AppNamedreactionByProducts.SingleOrDefault(i => i.ReactantId == reaction.Id);
                        _context.Remove(courseToRemove);
                    }
                }
            }
            PopulateReactantList(appNamedreaction);
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
            ViewData["CatalystId"] = new SelectList(_context.AppCatalyst, "Id", "Name", appNamedreaction.CatalystId);
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Name", appNamedreaction.FunctionalGroupId);
            ViewData["SolventId"] = new SelectList(_context.AppSolvent, "Id", "Name", appNamedreaction.SolventId);
            return View(appNamedreaction);
        }

        private void PopulateReactantList(AppNamedreaction reaction)
        {
            var allReactants = _context.AppReactant;
            var reactionReactants = new HashSet<long>(reaction.AppNamedreactionReactants.Select(c => c.ReactantId));
            var optionGroup = new SelectListGroup() { Name = "Reactant" };
            List<SelectListItem> items = new List<SelectListItem>();
            List<bool> selected = new List<bool>();
            foreach (var r in allReactants)
            {
                bool s = reactionReactants.Contains(r.Id);
                selected.Add(s);
                items.Add(new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name,
                    Selected = s,
                    Group = optionGroup
                });
            }
            var retVal = new MultiSelectList(items, "Value", "Text", selected);
            ViewData["Reactants"] = retVal;
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
                .Include(a => a.AppNamedreactionReactants)
                .Include(a => a.AppNamedreactionByProducts)
                .FirstOrDefaultAsync(m => m.Id == id);

            var fileName = _hostingEnvironment.WebRootPath + appNamedreaction.Image;
            if (System.IO.File.Exists(fileName)) System.IO.File.Exists(fileName);

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
