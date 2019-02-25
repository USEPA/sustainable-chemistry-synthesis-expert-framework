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

        public virtual SustainableChemistryWeb.Models.AppCatalyst Catalyst { get; set; }
        public virtual SustainableChemistryWeb.Models.AppFunctionalgroup FunctionalGroup { get; set; }
        public virtual SustainableChemistryWeb.Models.AppSolvent Solvent { get; set; }
        public virtual ICollection<SustainableChemistryWeb.Models.AppNamedreactionByProducts> AppNamedreactionByProducts { get; set; }
        public virtual ICollection<SustainableChemistryWeb.Models.AppNamedreactionReactants> AppNamedreactionReactants { get; set; }
        public virtual ICollection<SustainableChemistryWeb.Models.AppReference> AppReference { get; set; }
    }
}
