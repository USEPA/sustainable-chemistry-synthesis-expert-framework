using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SustainableChemistryWeb.ViewModels
{
    public class NamedReactionViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Product { get; set; }
        public string Heat { get; set; }
        public string AcidBase { get; set; }
        public string Image { get; set; }
        public long CatalystId { get; set; }
        public long FunctionalGroupId { get; set; }
        public long SolventId { get; set; }
        public string Url { get; set; }

        public virtual SustainableChemistryWeb.Models.Catalyst Catalyst { get; set; }
        public virtual SustainableChemistryWeb.Models.FunctionalGroup FunctionalGroup { get; set; }
        public virtual SustainableChemistryWeb.Models.Solvent Solvent { get; set; }
        public virtual ICollection<SustainableChemistryWeb.Models.NamedReactionByProducts> AppNamedreactionByProducts { get; set; }
        public virtual ICollection<SustainableChemistryWeb.Models.NamedReactionReactants> AppNamedreactionReactants { get; set; }
        public virtual ICollection<SustainableChemistryWeb.Models.Reference> AppReference { get; set; }
    }
}
