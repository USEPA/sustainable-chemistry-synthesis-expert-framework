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
    public class AppFunctionalgroupDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Smarts { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class SustainableChemistryController : ControllerBase
    {
        private readonly SustainableChemistryContext _context;

        public SustainableChemistryController(SustainableChemistryContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // GET: api/SustainableChemistry
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppFunctionalgroup>>> GetFunctionalGroupList()
        {
            var fG = await _context.AppFunctionalgroup.ToListAsync();
            var retVal = new List<AppFunctionalgroupDTO>();
            foreach (var group in fG)
            {
                retVal.Add(new AppFunctionalgroupDTO()
                {
                    Id = group.Id,
                    Name = group.Name,
                    Smarts = group.Smarts
                }
                );
            }
            return Ok(retVal);
        }

        // GET: api/SustainableChemistry/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppFunctionalgroup>> GetFunctionalGroup(long id)
        {
            var group = await _context.AppFunctionalgroup.FindAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            return Ok(new AppFunctionalgroupDTO()
            {
                Id = group.Id,
                Name = group.Name,
                Smarts = group.Smarts
            });
        }

        // GET: api/SustainableChemistry/bySmiles?smiles=O=P(OC)(OC)C
        [HttpGet("bySmiles")]
        public async Task<ActionResult<AppFunctionalgroup>> GetFunctionalGroups(string smiles)
        {
            var fG = await _context.AppFunctionalgroup.ToListAsync();
            ChemInfo.Molecule molecule = new ChemInfo.Molecule(smiles);
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
            return Ok(retVal);
        }

        // GET: api/SustainableChemistry/byMolecule?smiles=O=P(OC)(OC)C
        [HttpGet("byMolecule")]
        public async Task<ActionResult<AppFunctionalgroup>> GetMolecule(string smiles)
        {
            var fG = await _context.AppFunctionalgroup.ToListAsync();
            ChemInfo.Molecule molecule = new ChemInfo.Molecule(smiles);
            var retVal = new List<AppFunctionalgroupDTO>();
            foreach (var group in fG)
            {
                string smarts = group.Smarts;
                if (!string.IsNullOrEmpty(group.Smarts)) molecule.FindFunctionalGroup(group);
            }
            return Ok(molecule);
        }
    }
}