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
    // fields = ['Name', 'Functional_Group', 'Product', 'URL', 'Reactants', 'ByProducts', 'Heat', 'AcidBase', 'Catalyst', 'Solvent', 'Image']
    public class AppNamedreactionDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public AppFunctionalgroupDTO FunctionalGroup { get; set; }
        public string Product { get; set; }
        public string URL { get; set; }
        public ICollection<AppReactantDTO> Reactants { get; set; }
        public ICollection<AppReactantDTO> ByProducts { get; set; }
        public string Heat { get; set; }
        public string AcidBase { get; set; }
        public AppCatalystDTO Catalyst { get; set; }
        public AppSolventDTO Solvent { get; set; }
        public string Image { get; set; }
    }

    public class AppCatalystDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class AppSolventDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class AppReactantDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }



    [Route("api/[controller]")]
    [ApiController]
    public class NamedReactionController : ControllerBase
    {
        private readonly SustainableChemistryContext _context;

        public NamedReactionController(SustainableChemistryContext context)
        {
            _context = context;
        }

        // GET: api/namedreaction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppNamedreaction>>> GetNamedReactionList()
        {
            // return await _context.AppNamedreaction.ToListAsync();
            var nR = await _context.AppNamedreaction.ToListAsync();
            var retVal = new List<AppNamedreactionDTO>();
            foreach (var reaction in nR)
            {
                var funcGroup = await _context.AppFunctionalgroup.FindAsync(reaction.FunctionalGroupId);
                var catalyst = await _context.AppCatalyst.FindAsync(reaction.CatalystId);
                var solvent = await _context.AppSolvent.FindAsync(reaction.SolventId);
                var temp = from r in _context.AppNamedreactionReactants
                           where r.NamedreactionId == reaction.Id
                           select r;

                List<AppReactantDTO> reactants = new List<AppReactantDTO>();
                foreach (var r in temp)
                {
                    var react = await _context.AppReactant.FindAsync(r.ReactantId);
                    reactants.Add(new AppReactantDTO()
                    {
                        Id = r.ReactantId,
                        Name = react.Name
                    });
                }

                var temp2 = from r in _context.AppNamedreactionByProducts
                            where r.NamedreactionId == reaction.Id
                            select r;
                List<AppReactantDTO> byProducts = new List<AppReactantDTO>();
                foreach (var r in temp2)
                {
                    var react = await _context.AppReactant.FindAsync(r.ReactantId);
                    byProducts.Add(new AppReactantDTO()
                    {
                        Id = r.ReactantId,
                        Name = react.Name
                    });
                }

                if (reaction == null)
                {
                    return NotFound();
                }

                retVal.Add(new AppNamedreactionDTO()
                {
                    Id = reaction.Id,
                    Name = reaction.Name,
                    FunctionalGroup = new AppFunctionalgroupDTO()
                    {
                        Id = funcGroup.Id,
                        Name = funcGroup.Name,
                        Smarts = funcGroup.Smarts
                    },
                    URL = reaction.Url,
                    Product = reaction.Product,
                    Heat = reaction.Heat,
                    AcidBase = reaction.AcidBase,
                    Catalyst = new AppCatalystDTO()
                    {
                        Id = catalyst.Id,
                        Name = catalyst.Name,
                    },
                    Solvent = new AppSolventDTO()
                    {
                        Id = solvent.Id,
                        Name = solvent.Name,
                    },
                    Image = reaction.Image,
                    Reactants = reactants,
                    ByProducts = byProducts
                }
                );
            }
            return Ok(retVal);
        }

        // GET: /namedreaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AppNamedreaction>> GetNamedReaction(long id)
        {
            //return await _context.AppNamedreaction.FindAsync(id);

            var reaction = await _context.AppNamedreaction.FindAsync(id);
            var funcGroup = await _context.AppFunctionalgroup.FindAsync(reaction.FunctionalGroupId);
            var catalyst = await _context.AppCatalyst.FindAsync(reaction.CatalystId);
            var solvent = await _context.AppSolvent.FindAsync(reaction.SolventId);
            var temp = from r in _context.AppNamedreactionReactants
                       where r.NamedreactionId == id
                       select r;

            List<AppReactantDTO> reactants = new List<AppReactantDTO>();
            foreach (var r in temp)
            {
                var react = await _context.AppReactant.FindAsync(r.ReactantId);
                reactants.Add(new AppReactantDTO()
                {
                    Id = r.ReactantId,
                    Name = react.Name
                });
            }

            var temp2 = from r in _context.AppNamedreactionByProducts
                        where r.NamedreactionId == id
                        select r;
            List<AppReactantDTO> byProducts = new List<AppReactantDTO>();
            foreach (var r in temp2)
            {
                var react = await _context.AppReactant.FindAsync(r.ReactantId);
                byProducts.Add(new AppReactantDTO()
                {
                    Id = r.ReactantId,
                    Name = react.Name
                });
            }

            if (reaction == null)
            {
                return NotFound();
            }

            return Ok(new AppNamedreactionDTO()
            {
                Id = reaction.Id,
                Name = reaction.Name,
                FunctionalGroup = new AppFunctionalgroupDTO()
                {
                    Id = funcGroup.Id,
                    Name = funcGroup.Name,
                    Smarts = funcGroup.Smarts
                },
                URL = reaction.Url,
                Product = reaction.Product,
                Heat = reaction.Heat,
                AcidBase = reaction.AcidBase,
                Catalyst = new AppCatalystDTO()
                {
                    Id = catalyst.Id,
                    Name = catalyst.Name,
                },
                Solvent = new AppSolventDTO()
                {
                    Id = solvent.Id,
                    Name = solvent.Name,
                },
                Image = reaction.Image,
                Reactants = reactants,
                ByProducts = byProducts
            });
        }

        // GET: /namedreaction/byFunctionalGroup?id=173
        [HttpGet("byFunctionalGroup")]
        public async Task<ActionResult<AppNamedreaction>> GetNamedReactionByFuncationGroupId(long id)
        {
            // return await _context.AppNamedreaction.ToListAsync();
            var nR = from r in _context.AppNamedreaction
                     where r.FunctionalGroupId == id
                     select r;
            var retVal = new List<AppNamedreactionDTO>();
            foreach (var reaction in nR)
            {
                var funcGroup = await _context.AppFunctionalgroup.FindAsync(reaction.FunctionalGroupId);
                var catalyst = await _context.AppCatalyst.FindAsync(reaction.CatalystId);
                var solvent = await _context.AppSolvent.FindAsync(reaction.SolventId);
                var temp = from r in _context.AppNamedreactionReactants
                           where r.NamedreactionId == reaction.Id
                           select r;

                List<AppReactantDTO> reactants = new List<AppReactantDTO>();
                foreach (var r in temp)
                {
                    var react = await _context.AppReactant.FindAsync(r.ReactantId);
                    reactants.Add(new AppReactantDTO()
                    {
                        Id = r.ReactantId,
                        Name = react.Name
                    });
                }

                var temp2 = from r in _context.AppNamedreactionByProducts
                            where r.NamedreactionId == reaction.Id
                            select r;
                List<AppReactantDTO> byProducts = new List<AppReactantDTO>();
                foreach (var r in temp2)
                {
                    var react = await _context.AppReactant.FindAsync(r.ReactantId);
                    byProducts.Add(new AppReactantDTO()
                    {
                        Id = r.ReactantId,
                        Name = react.Name
                    });
                }

                if (reaction == null)
                {
                    return NotFound();
                }

                retVal.Add(new AppNamedreactionDTO()
                {
                    Id = reaction.Id,
                    Name = reaction.Name,
                    FunctionalGroup = new AppFunctionalgroupDTO()
                    {
                        Id = funcGroup.Id,
                        Name = funcGroup.Name,
                        Smarts = funcGroup.Smarts
                    },
                    URL = reaction.Url,
                    Product = reaction.Product,
                    Heat = reaction.Heat,
                    AcidBase = reaction.AcidBase,
                    Catalyst = new AppCatalystDTO()
                    {
                        Id = catalyst.Id,
                        Name = catalyst.Name,
                    },
                    Solvent = new AppSolventDTO()
                    {
                        Id = solvent.Id,
                        Name = solvent.Name,
                    },
                    Image = reaction.Image,
                    Reactants = reactants,
                    ByProducts = byProducts
                }
                );
            }
            return Ok(retVal);
        }
    }
}