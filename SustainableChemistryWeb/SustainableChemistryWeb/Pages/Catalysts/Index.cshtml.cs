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
    public class IndexModel : PageModel
    {
        private readonly SustainableChemistryWeb.Models.SustainableChemistryContext _context;

        public IndexModel(SustainableChemistryWeb.Models.SustainableChemistryContext context)
        {
            _context = context;
        }

        public IList<Catalyst> Catalyst { get;set; }

        public async Task OnGetAsync()
        {
            Catalyst = await _context.AppCatalyst.ToListAsync();
        }
    }
}
