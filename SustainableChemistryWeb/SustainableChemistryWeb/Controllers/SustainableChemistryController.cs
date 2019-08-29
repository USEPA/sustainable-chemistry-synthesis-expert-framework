using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SustainableChemistryWeb.Controllers
{
    public class AppFunctionalgroupDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Smarts { get; set; }
    }

    [Route("[controller]")]
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
        // GET: /SustainableChemistry/GetFunctionalGroupList
        [HttpGet("GetFunctionalGroupList")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<IEnumerable<FunctionalGroup>>> GetFunctionalGroupList()
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

        // GET: /SustainableChemistry/GetFunctionalGroup/5
        [HttpGet("GetFunctionalGroup/{id}")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<FunctionalGroup>> GetFunctionalGroup(long id)
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

        // GET: /SustainableChemistry/GetFunctionalGroups?smiles=O=P(OC)(OC)C
        [HttpGet("GetFunctionalGroups")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<FunctionalGroup>> GetFunctionalGroups(string smiles)
        {
            var fG = await _context.AppFunctionalgroup.ToListAsync();
            ChemInfo.Molecule molecule = new ChemInfo.Molecule(smiles);
            var retVal = new List<AppFunctionalgroupDTO>();
            foreach (var group in fG)
            {
                string smarts = group.Smarts;
                if (!string.IsNullOrEmpty(group.Smarts))
                    if (molecule.FindFunctionalGroup(group))
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

        // GET: /SustainableChemistry/GetMolecule?smiles=O=P(OC)(OC)C
        [HttpGet("GetMolecule")]
        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        public async Task<ActionResult<FunctionalGroup>> GetMolecule(string smiles)
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