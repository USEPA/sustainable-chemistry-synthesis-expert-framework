using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;

namespace SustainableChemistryWeb.Views.Catalysts
{
    public class DeleteModel : PageModel
    {
        private readonly SustainableChemistryWeb.Models.SustainableChemistryContext _context;

        public DeleteModel(SustainableChemistryWeb.Models.SustainableChemistryContext context)
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

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Catalyst = await _context.AppCatalyst.FindAsync(id);

            if (Catalyst != null)
            {
                _context.AppCatalyst.Remove(Catalyst);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
