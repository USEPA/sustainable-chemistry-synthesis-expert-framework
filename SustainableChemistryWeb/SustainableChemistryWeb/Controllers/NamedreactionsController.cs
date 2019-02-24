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
        public async Task<IActionResult> Create([Bind("Id,Name,ReactantA,ReactantB,ReactantC,Product,Heat,AcidBase,Image,CatalystId,FunctionalGroupId,SolventId,Url")] AppNamedreaction appNamedreaction)
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
            PopulateAssignedCourseData(appNamedreaction);
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
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,ReactantA,ReactantB,ReactantC,Product,Heat,AcidBase,Image,CatalystId,FunctionalGroupId,SolventId,Url")] AppNamedreaction appNamedreaction)
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

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appNamedreaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppNamedreactionExists(appNamedreaction.Id))
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
            PopulateAssignedCourseData(appNamedreaction);
            ViewData["CatalystId"] = new SelectList(_context.AppCatalyst, "Id", "Name", appNamedreaction.CatalystId);
            ViewData["FunctionalGroupId"] = new SelectList(_context.AppFunctionalgroup, "Id", "Name", appNamedreaction.FunctionalGroupId);
            ViewData["SolventId"] = new SelectList(_context.AppSolvent, "Id", "Name", appNamedreaction.SolventId);
            return View(appNamedreaction);
        }

        private void PopulateAssignedCourseData(AppNamedreaction reaction)
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
            var appNamedreaction = await _context.AppNamedreaction.FindAsync(id);
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
