using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;

namespace SustainableChemistryWeb.Views.Catalysts
{
    public class EditModel : PageModel
    {
        private readonly SustainableChemistryWeb.Models.SustainableChemistryContext _context;

        public EditModel(SustainableChemistryWeb.Models.SustainableChemistryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Catalyst Catalyst { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Catalyst = await _context.AppCatalyst.FirstOrDefaultAsync(m => m.Id == id);

            if (Catalyst == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Catalyst).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CatalystExists(Catalyst.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CatalystExists(long id)
        {
            return _context.AppCatalyst.Any(e => e.Id == id);
        }
    }
}
