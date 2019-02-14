using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;

namespace SustainableChemistryWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceController : ControllerBase
    {
        private readonly SustainableChemistryContext _context;

        public ReferenceController(SustainableChemistryContext context)
        {
            _context = context;
        }

        // GET: api/references
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppReference>>> GetReferenceGroupList()
        {
            var references = await _context.AppReference.ToListAsync();
            var retVal = new List<SustainableChemistryWeb.ChemInfo.Reference>();
            foreach (var r in references)
            {
                if (r.FunctionalGroupId != null && r.ReactionId != null)
                {
                    retVal.Add(new SustainableChemistryWeb.ChemInfo.Reference(r.FunctionalGroupId.Value, r.ReactionId.Value, r.Risdata));
                }
            }
            return Ok(retVal);
        }

        // GET: api/reference/173
        [HttpGet("{id}")]
        public async Task<ActionResult<AppFunctionalgroup>> GetReference(long id)
        {
            var r = await _context.AppReference.FindAsync(id);

            if (r == null)
            {
                return NotFound();
            }

            if (r.FunctionalGroupId == null)
            {
                return NotFound();
            }

            if (r.ReactionId == null)
            {
                return NotFound();
            }

            return Ok(new SustainableChemistryWeb.ChemInfo.Reference(r.FunctionalGroupId.Value, r.ReactionId.Value, r.Risdata));
        }

        // GET: api/reference/ByNamedReaction?Id=143
        [HttpGet("{ByNamedReaction}")]
        public async Task<ActionResult<AppFunctionalgroup>> GetReferenceByNamedReaction(long id)
        {
            var refs = await _context.AppReference.ToListAsync();
            var references = from r in refs
                     where r.ReactionId == id
                     select r;
            var retVal = new List<SustainableChemistryWeb.ChemInfo.Reference>();
            foreach (var r in references)
            {
                if (r.FunctionalGroupId != null && r.ReactionId != null)
                {
                    retVal.Add(new SustainableChemistryWeb.ChemInfo.Reference(r.FunctionalGroupId.Value, r.ReactionId.Value, r.Risdata));
                }
            }
            return Ok(retVal);
        }

        // GET: api/reference/ByFunctionalGroup?Id=173
        [HttpGet("{ByFunctionalGroup}")]
        public async Task<ActionResult<AppFunctionalgroup>> GetReferenceByFunctionalGroup(long id)
        {
            var refs = await _context.AppReference.ToListAsync();
            var references = from r in refs
                             where r.FunctionalGroupId == id
                             select r;
            var retVal = new List<SustainableChemistryWeb.ChemInfo.Reference>();
            foreach (var r in references)
            {
                if (r.FunctionalGroupId != null && r.ReactionId != null)
                {
                    retVal.Add(new SustainableChemistryWeb.ChemInfo.Reference(r.FunctionalGroupId.Value, r.ReactionId.Value, r.Risdata));
                }
            }
            return Ok(retVal);
        }
    }
}