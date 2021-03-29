using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SustainableChemistryWeb.Controllers
{
    public class SolventsController : Controller
    {
        private readonly SustainableChemistryContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;

        public SolventsController(SustainableChemistryContext context, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Solvents
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppSolvent.OrderBy(i => i.Name.ToLower()).ToListAsync());
        }

        // GET: Solvents/Details/5
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appSolvent = await _context.AppSolvent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appSolvent == null)
            {
                return NotFound();
            }

            return View(appSolvent);
        }

        // GET: Solvents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Solvents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] SustainableChemistryWeb.Models.Solvent solvent)
        {
            try
            {
                Solvent appSolvent = new Solvent()
                {
                    Name = solvent.Name,
                    Description = solvent.Description
                };
                if (ModelState.IsValid)
                {
                    _context.Add(appSolvent);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Solvents/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appSolvent = await _context.AppSolvent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appSolvent == null)
            {
                return NotFound();
            }

            return View(appSolvent);
        }

        // POST: Solvents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("Id,Name,Description")] SustainableChemistryWeb.Models.Solvent solvent)
        {
            var solventToUpdate = await _context.AppSolvent
                .SingleOrDefaultAsync(m => m.Id == id);
            if (await TryUpdateModelAsync<Solvent>(
                           solventToUpdate,
                           "",
                           r => r.Name, r => r.Description))
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return View();
                }
            return RedirectToAction(nameof(Index));
        }

        // GET: Solvents/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appSolvent = await _context.AppSolvent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appSolvent == null)
            {
                return NotFound();
            }

            return View(appSolvent);
        }

        // POST: Solvents/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long? id, IFormCollection collection)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appSolvent = await _context.AppSolvent
                .FirstOrDefaultAsync(m => m.Id == id);

            _context.AppSolvent.Remove(appSolvent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}