using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class AppReactant
    {
        public AppReactant()
        {
            AppNamedreactionByProducts = new HashSet<AppNamedreactionByProducts>();
            AppNamedreactionReactants = new HashSet<AppNamedreactionReactants>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Temp2 { get; set; }

        public virtual ICollection<AppNamedreactionByProducts> AppNamedreactionByProducts { get; set; }
        public virtual ICollection<AppNamedreactionReactants> AppNamedreactionReactants { get; set; }
    }
}
