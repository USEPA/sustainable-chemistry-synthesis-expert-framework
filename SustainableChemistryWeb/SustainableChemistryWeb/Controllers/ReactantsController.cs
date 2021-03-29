using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SustainableChemistryWeb.Controllers
{
    public class ReactantsController : Controller
    {
        private readonly SustainableChemistryContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;

        public ReactantsController(SustainableChemistryContext context, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Reactants
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppReactant.OrderBy(i => i.Name.ToLower()).ToListAsync());
        }

        // GET: Reactants/Details/5
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appSolvent = await _context.AppReactant
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appSolvent == null)
            {
                return NotFound();
            }

            return View(appSolvent);
        }

        // GET: Reactants/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Reactants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] SustainableChemistryWeb.Models.Reactant reactant)
        {
            try
            {
                Reactant appReactant = new Reactant()
                {
                    Name = reactant.Name,
                    Description = reactant.Description,
                    Temp2 = string.Empty,
                };
                if (string.IsNullOrEmpty(appReactant.Description)) appReactant.Description = string.Empty;
                if (ModelState.IsValid)
                {
                    _context.Add(appReactant);
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

        // GET: Reactants/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appReactant = await _context.AppReactant
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appReactant == null)
            {
                return NotFound();
            }

            return View(appReactant);
        }

        // POST: Reactants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] SustainableChemistryWeb.Models.Reactant reactant)
        {
            var reactantToUpdate = await _context.AppReactant
               .SingleOrDefaultAsync(m => m.Id == id);
            if (string.IsNullOrEmpty(reactant.Description)) reactant.Description = string.Empty;
            if (await TryUpdateModelAsync<Reactant>(
                           reactantToUpdate,
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

        // GET: Reactants/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appSolvent = await _context.AppReactant
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appSolvent == null)
            {
                return NotFound();
            }

            return View(appSolvent);
        }

        // POST: Reactants/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, IFormCollection collection)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appReactant = await _context.AppReactant
                .FirstOrDefaultAsync(m => m.Id == id);

            _context.AppReactant.Remove(appReactant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}