using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class AppNamedreaction
    {
        public AppNamedreaction()
        {
            AppNamedreactionByProducts = new HashSet<AppNamedreactionByProducts>();
            AppNamedreactionReactants = new HashSet<AppNamedreactionReactants>();
            AppReference = new HashSet<AppReference>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string ReactantA { get; set; }
        public string ReactantB { get; set; }
        public string ReactantC { get; set; }
        public string Product { get; set; }
        public string Heat { get; set; }
        public string AcidBase { get; set; }
        public string Image { get; set; }
        public long CatalystId { get; set; }
        public long FunctionalGroupId { get; set; }
        public long SolventId { get; set; }
        public string Url { get; set; }

        public virtual AppCatalyst Catalyst { get; set; }
        public virtual AppFunctionalgroup FunctionalGroup { get; set; }
        public virtual AppSolvent Solvent { get; set; }
        public virtual ICollection<AppNamedreactionByProducts> AppNamedreactionByProducts { get; set; }
        public virtual ICollection<AppNamedreactionReactants> AppNamedreactionReactants { get; set; }
        public virtual ICollection<AppReference> AppReference { get; set; }
    }
}
