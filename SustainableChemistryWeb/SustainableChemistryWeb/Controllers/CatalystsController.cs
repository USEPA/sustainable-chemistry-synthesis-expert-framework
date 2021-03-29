using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SustainableChemistryWeb.Controllers
{
    public class CatalystsController : Controller
    {
        private readonly SustainableChemistryContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;

        public CatalystsController(SustainableChemistryContext context, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: Catalysts
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppCatalyst.OrderBy(i => i.Name.ToLower()).ToListAsync());
        }

        // GET: Catalysts/Details/5
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appCatalyst = await _context.AppCatalyst
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appCatalyst == null)
            {
                return NotFound();
            }

            return View(appCatalyst);
        }

        // GET: Catalysts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catalysts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] SustainableChemistryWeb.Models.Catalyst catalyst)
        {
            try
            {
                Catalyst appCat = new Catalyst()
                {
                    Name = catalyst.Name,
                    Description = catalyst.Description
                };
                if (ModelState.IsValid)
                {
                    _context.Add(appCat);
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

        // GET: Catalysts/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appCatalyst = await _context.AppCatalyst
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appCatalyst == null)
            {
                return NotFound();
            }

            return View(appCatalyst);
        }

        // POST: Catalysts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("Id,Name,Description")] SustainableChemistryWeb.Models.Catalyst catalyst)
        {

            var catalystToUpdate = await _context.AppCatalyst
                .SingleOrDefaultAsync(m => m.Id == id);
            if (await TryUpdateModelAsync<Catalyst>(
                           catalystToUpdate,
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

        // GET: Catalysts/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appCatalyst = await _context.AppCatalyst
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appCatalyst == null)
            {
                return NotFound();
            }

            return View(appCatalyst);
        }

        // POST: Catalysts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long? id, IFormCollection collection)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appCatalyst = await _context.AppCatalyst
                .FirstOrDefaultAsync(m => m.Id == id);

            _context.AppCatalyst.Remove(appCatalyst);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}