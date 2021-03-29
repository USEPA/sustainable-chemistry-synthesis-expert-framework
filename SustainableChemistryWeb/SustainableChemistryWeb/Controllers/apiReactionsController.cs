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
    public class ReactionsController : ControllerBase
    {
        private readonly SustainableChemistryContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _hostingEnvironment;

        public ReactionsController(SustainableChemistryContext context, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: api/Reactions
        [HttpGet]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NamedReactionAPI>>> GetReactionItems()
        {
            var retVal = new List<NamedReactionAPI>();
            var reactions = await _context.AppNamedreaction
                .Include(a => a.Catalyst)
                .Include(a => a.FunctionalGroup)
                .Include(a => a.Solvent)
                .Include(a => a.AppNamedreactionReactants)
                    .ThenInclude(a => a.Reactant)
                .Include(a => a.AppNamedreactionByProducts)
                    .ThenInclude(a => a.Reactant)
                .ToListAsync();
            foreach (NamedReaction rxn in reactions)
            {
                retVal.Add(new NamedReactionAPI(rxn));
            }
            return retVal;
        }

        // GET: api/Reactions/5
        [HttpGet("{id}")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<NamedReactionAPI>> GetById(long id)
        {
            var rxn = await _context.AppNamedreaction
                .Include(a => a.Catalyst)
                .Include(a => a.FunctionalGroup)
                .Include(a => a.Solvent)
                .Include(a => a.AppNamedreactionReactants)
                    .ThenInclude(a => a.Reactant)
                .Include(a => a.AppNamedreactionByProducts)
                    .ThenInclude(a => a.Reactant)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rxn == null)
            {
                return NotFound();
            }
            return new NamedReactionAPI(rxn);
        }

        // GET: api/Reactions/FG/ALCOHOL or api/Reactions/FG/5
        [HttpGet("[action]/{id}")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<IEnumerable<NamedReactionAPI>>> FG(string id)
        {
            var retVal = new List<NamedReactionAPI>();
            if (long.TryParse(id, out long key))
            {
                var temp = await _context.AppNamedreaction
                    .Include(a => a.Catalyst)
                    .Include(a => a.FunctionalGroup)
                    .Include(a => a.Solvent)
                    .Include(a => a.AppNamedreactionReactants)
                    .ThenInclude(a => a.Reactant)
                    .Include(a => a.AppNamedreactionByProducts)
                    .ThenInclude(a => a.Reactant)
                    .Where(a => a.FunctionalGroupId == key)
                    .ToListAsync();
                foreach (NamedReaction rxn in temp)
                {
                    retVal.Add(new NamedReactionAPI(rxn));
                }
                return retVal;

            }
            var reactions = await _context.AppNamedreaction
                .Include(a => a.Catalyst)
                .Include(a => a.FunctionalGroup)
                .Include(a => a.Solvent)
                .Include(a => a.AppNamedreactionReactants)
                    .ThenInclude(a => a.Reactant)
                .Include(a => a.AppNamedreactionByProducts)
                    .ThenInclude(a => a.Reactant)
                .Where(a => a.FunctionalGroup.Name == id)
                .ToListAsync();
            foreach (NamedReaction rxn in reactions)
            {
                retVal.Add(new NamedReactionAPI(rxn));
            }
            return retVal;
        }


    }
}