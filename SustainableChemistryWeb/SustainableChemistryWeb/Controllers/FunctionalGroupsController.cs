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
    public class FunctionalGroupsController : Controller
    {
        private readonly SustainableChemistryContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public FunctionalGroupsController(SustainableChemistryContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: FunctionalGroups

        public async Task<IActionResult> Index(int? Id, int? namedReactionId, string nameSearchString, string smilesSearchString)
        {
            var viewModel = new SustainableChemistryWeb.ViewModels.FunctionalGroupIndexData();
            viewModel.FunctionalGroups = await _context.AppFunctionalgroup
                  .Include(i => i.AppNamedreaction)
                  .Include(i => i.AppReference)
                  .AsNoTracking()
                  .OrderBy(i => i.Name)
                  .ToListAsync();

            if (!String.IsNullOrEmpty(nameSearchString))
            {
                viewModel.FunctionalGroups = viewModel.FunctionalGroups.Where(s => s.Name.Contains(nameSearchString, StringComparison.OrdinalIgnoreCase));
            }

            if (!String.IsNullOrEmpty(smilesSearchString)) await Task.Run(() =>
            {
                ChemInfo.Molecule molecule = new ChemInfo.Molecule(smilesSearchString.Trim());
                List<FunctionalGroup> fgName = new List<FunctionalGroup>();
                foreach (var fg in viewModel.FunctionalGroups)
                {
                    string smarts = fg.Smarts;
                    if (!string.IsNullOrEmpty(fg.Smarts))
                        if (molecule.FindFunctionalGroup(fg.Smarts))
                        {
                            fgName.Add(fg);
                        }
                    viewModel.FunctionalGroups = fgName;
                }
            });
            if (Id != null)
            {
                ViewData["FunctionalGroupID"] = Id.Value;
                FunctionalGroup funcGroup = viewModel.FunctionalGroups.Where(
                    i => i.Id == Id.Value).Single();
                viewModel.NamedReactions = funcGroup.AppNamedreaction.Select(s => s.Name);
            }

            //if (namedReactionId != null)
            //{
            //    ViewData["NamedReactionID"] = namedReactionId.Value;
            //    viewModel.NamedReactions = viewModel.NamedReactions.Where(
            //        x => x.Id == namedReactionId).Single().Name;
            //}

            return View(viewModel);

            //return View(retVal.ToAsyncEnumerable());
        }

        // GET: FunctionalGroups/Details/5
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
        public async Task<IActionResult> Create([Bind("Id,Name,Smarts,Image")] SustainableChemistryWeb.ViewModels.FunctionalGroupViewModel functionalGroupView)
        {
            string name = System.IO.Path.GetFileName(functionalGroupView.Image.FileName);
            FunctionalGroup appFunctionalGroup = new FunctionalGroup()
            {
                Name = functionalGroupView.Name,
                Smarts = functionalGroupView.Smarts,
                Image = "Images/FunctionalGroups/" + name,
            };
            if (functionalGroupView.Image.Length > 0)
            {
                using (var stream = new System.IO.FileStream(_hostingEnvironment.WebRootPath + "/Images/FunctionalGroups/" + name, System.IO.FileMode.Create))
                {
                    await functionalGroupView.Image.CopyToAsync(stream);
                    stream.Close();
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(appFunctionalGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appFunctionalGroup);
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
                ImageFileName = appFunctionalgroup.Image,
            };

            return View(functionalGroupView);
        }

        // POST: FunctionalGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Smarts,Image")] SustainableChemistryWeb.ViewModels.FunctionalGroupViewModel functionalGroupView)
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
                r => r.Name, r => r.Smarts, r => r.Smarts))
            {
                try
                {
                    var fileName = _hostingEnvironment.WebRootPath + "/" + functionalGroupToUpdate.Image;
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }

                    if (functionalGroupView.Image != null)
                    {
                        string name = System.IO.Path.GetFileName(functionalGroupView.Image.FileName);
                        using (var stream = new System.IO.FileStream(_hostingEnvironment.WebRootPath + "/Images/FunctionalGroups/" + name, System.IO.FileMode.Create))
                        {
                            functionalGroupView.Image.CopyTo(stream);
                            stream.Close();
                        }
                        functionalGroupToUpdate.Image = "Images/FunctionalGroups/" + name;
                    }
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
            return View(functionalGroupToUpdate);
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
