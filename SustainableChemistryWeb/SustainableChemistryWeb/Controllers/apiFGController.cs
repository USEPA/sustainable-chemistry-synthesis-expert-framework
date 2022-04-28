using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;
using SustainableChemistryWeb.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SustainableChemistryWeb.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FGController : ControllerBase
    {
        private readonly SustainableChemistryContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;

        public FGController(SustainableChemistryContext context, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/FG
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<IEnumerable<FunctionalGroupAPI>>> GetFunctionalGroupItems()
        {
            List<FunctionalGroupAPI> retVal = new List<FunctionalGroupAPI>();
            var fgs = await _context.AppFunctionalgroup.ToListAsync();
            foreach (var fg in fgs)
            {
                retVal.Add(new FunctionalGroupAPI
                {
                    Id = fg.Id,
                    Name = fg.Name,
                    Smarts = fg.Smarts,
                    URL = fg.URL,
                    Image = fg.Image,
                });
            }
            return retVal;

        }

        // GET: api/FG/5
        [HttpGet("{id}")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<FunctionalGroupAPI>> GetById(long id)
        {
            var functionalGroup = await _context.AppFunctionalgroup.FindAsync(id);

            if (functionalGroup == null)
            {
                return NotFound();
            }

            return new FunctionalGroupAPI
            {
                Id = functionalGroup.Id,
                Name = functionalGroup.Name,
                Smarts = functionalGroup.Smarts,
                URL = functionalGroup.URL,
                Image = functionalGroup.Image,
            };
        }

        // GET: api/FG/Smiles/O=P(OC)(OC)C
        [HttpGet("[action]/{id}")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<IEnumerable<FunctionalGroupAPI>>> Smiles(string id)
        {
            var fgList = await _context.AppFunctionalgroup.ToListAsync();
            List<FunctionalGroup> fgFound = new List<FunctionalGroup>();
            ChemInfo.Molecule molecule = new ChemInfo.Molecule(id.Trim());
            if (molecule == null)
            {
                return NotFound();
            }
            if (molecule.Atoms.Length != 0)
            {
                foreach (var fg in fgList)
                {
                    string smarts = fg.Smarts;
                    if (!string.IsNullOrEmpty(fg.Smarts))
                        if (molecule.FindFunctionalGroup(fg))
                        {
                            fgFound.Add(fg);
                        }
                }
                if (molecule.Aromatic) fgFound.Add(_context.AppFunctionalgroup
                    .FirstOrDefault(m => m.Id == 35));
                if (molecule.Heterocyclic) fgFound.Add(_context.AppFunctionalgroup
                    .FirstOrDefault(m => m.Id == 118));
                if (molecule.HeterocyclicAromatic) fgFound.Add(_context.AppFunctionalgroup
                    .FirstOrDefault(m => m.Id == 224));
            }
            List<FunctionalGroupAPI> retVal = new List<FunctionalGroupAPI>();
            foreach (var fg in fgFound)
            {
                retVal.Add(new FunctionalGroupAPI
                {
                    Id = fg.Id,
                    Name = fg.Name,
                    Smarts = fg.Smarts,
                    URL = fg.URL,
                    Image = fg.Image,
                });
            }
            return retVal;
        }

        // GET: api/FG/Smiles/O=P(OC)(OC)C
        [HttpGet("[action]/{id}")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<NamedReactionViewModel>> Reaction(int id)
        {
            var appNamedreaction = await _context.AppNamedreaction
                .Include(a => a.Catalyst)
                .Include(a => a.FunctionalGroup)
                .Include(a => a.Solvent)
                .Include(a => a.AppNamedreactionReactants)
                    .ThenInclude(a => a.Reactant)
                .Include(a => a.AppNamedreactionByProducts)
                    .ThenInclude(a => a.Reactant)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appNamedreaction == null)
            {
                return NotFound();
            }

            var reactionViewModel = new SustainableChemistryWeb.ViewModels.NamedReactionViewModel
            {
                Id = appNamedreaction.Id,
                Name = appNamedreaction.Name,
                FunctionalGroup = appNamedreaction.FunctionalGroup,
                Catalyst = appNamedreaction.Catalyst,
                Solvent = appNamedreaction.Solvent,
                Product = appNamedreaction.Product,
                AppNamedreactionByProducts = appNamedreaction.AppNamedreactionByProducts,
                AppNamedreactionReactants = appNamedreaction.AppNamedreactionReactants,
                AppReference = appNamedreaction.AppReference,
                Url = appNamedreaction.Url,
                Image = appNamedreaction.Image,
            };

            if (appNamedreaction.Heat == "HE") reactionViewModel.Heat = "Heat";
            else reactionViewModel.Heat = "Not Applicable";

            if (appNamedreaction.AcidBase == "AC") reactionViewModel.AcidBase = "Acid";
            else if (appNamedreaction.AcidBase == "BA") reactionViewModel.AcidBase = "Base";
            else if (appNamedreaction.AcidBase == "AB") reactionViewModel.AcidBase = "Acid Or Base";
            else reactionViewModel.AcidBase = "Not Applicable";

            return reactionViewModel;
        }
    }
}