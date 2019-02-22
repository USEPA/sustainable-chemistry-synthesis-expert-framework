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
        public async Task<IActionResult> Index(string nameSearchString, string smilesSearchString)
        {
            var retVal = new List<AppFunctionalgroup>();
            var groups = from s in _context.AppFunctionalgroup
                            select s;

            if (!String.IsNullOrEmpty(nameSearchString))
            {
                groups = groups.Where(s => s.Name.Contains(nameSearchString, StringComparison.OrdinalIgnoreCase));
            }

            if (!String.IsNullOrEmpty(smilesSearchString)) await Task.Run(() =>
            {
                ChemInfo.Molecule molecule = new ChemInfo.Molecule(smilesSearchString);
                foreach (var fg in groups)
                {
                    string smarts = fg.Smarts;
                    if (!string.IsNullOrEmpty(fg.Smarts))
                        if (molecule.FindFunctionalGroup(fg.Smarts))
                        {
                            retVal.Add(fg);
                        }
                }
            });
            else retVal.AddRange(groups.ToList());

            return View(retVal.ToAsyncEnumerable());
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
        public async Task<IActionResult> Create([Bind("Id,Name,Smarts,Image")] AppFunctionalgroup appFunctionalgroup)
        {
            if (appFunctionalgroup.Image != null && appFunctionalgroup.Image.Length > 0)
            {
                var file = System.IO.File.Open(appFunctionalgroup.Image, System.IO.FileMode.Open);
                appFunctionalgroup.Image = "Images/FunctionalGroups/" + System.IO.Path.GetFileName(file.Name);
                //There is an error here
                //var uploads = System.IO.Path.Combine(_appEnvironment.WebRootPath, "uploads\\img");
                if (file.Length > 0)
                {
                    var fileName = _hostingEnvironment.WebRootPath + "\\Images\\FunctionalGroups\\" + System.IO.Path.GetFileName(file.Name);
                    using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                file.Close();
            }

            if (ModelState.IsValid)
            {
                _context.Add(appFunctionalgroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(appFunctionalgroup);
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
            return View(appFunctionalgroup);
        }

        // POST: FunctionalGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Smarts,Image")] AppFunctionalgroup appFunctionalgroup)
        {
            if (id != appFunctionalgroup.Id)
            {
                return NotFound();
            }

            if (appFunctionalgroup.Image != null && appFunctionalgroup.Image.Length > 0)
            {
                var file = System.IO.File.Open(appFunctionalgroup.Image, System.IO.FileMode.Open);
                appFunctionalgroup.Image = "Images/FunctionalGroups/" + System.IO.Path.GetFileName(file.Name); 
                //There is an error here
                //var uploads = System.IO.Path.Combine(_appEnvironment.WebRootPath, "uploads\\img");
                if (file.Length > 0)
                {
                    var fileName = _hostingEnvironment.WebRootPath + "\\Images\\FunctionalGroups\\" + System.IO.Path.GetFileName(file.Name);
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
                    _context.Update(appFunctionalgroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppFunctionalgroupExists(appFunctionalgroup.Id))
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
            return View(appFunctionalgroup);
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
