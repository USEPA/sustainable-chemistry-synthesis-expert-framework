using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SustainableChemistryWeb.Models;
using System.Text.Encodings.Web;
using Microsoft.EntityFrameworkCore;

namespace SustainableChemistryWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly SustainableChemistryContext _context;

        public HomeController(SustainableChemistryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // fg?smiles=O=P(OC)(OC)C
        public async Task<IActionResult> FG(string smiles)
        {
            ChemInfo.Molecule molecule = new ChemInfo.Molecule(smiles);
            var fG = from f in _context.AppFunctionalgroup select f;
            var retVal = new List<AppFunctionalgroupDTO>();
            foreach (var group in fG)
            {
                string smarts = group.Smarts;
                if (!string.IsNullOrEmpty(group.Smarts))
                    if (molecule.FindFunctionalGroup(group.Smarts))
                    {
                        retVal.Add(new AppFunctionalgroupDTO()
                        {
                            Id = group.Id,
                            Name = group.Name,
                            Smarts = group.Smarts
                        }
                        );
                    }
            }
            return View(retVal);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
