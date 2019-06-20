using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;

namespace SustainableChemistryWeb.Views.Solvents
{
    public class EditModel : PageModel
    {
        private readonly SustainableChemistryWeb.Models.SustainableChemistryContext _context;

        public EditModel(SustainableChemistryWeb.Models.SustainableChemistryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Solvent Solvent { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Solvent = await _context.AppSolvent.FirstOrDefaultAsync(m => m.Id == id);

            if (Solvent == null)
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

            _context.Attach(Solvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolventExists(Solvent.Id))
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

        private bool SolventExists(long id)
        {
            return _context.AppSolvent.Any(e => e.Id == id);
        }
    }
}
