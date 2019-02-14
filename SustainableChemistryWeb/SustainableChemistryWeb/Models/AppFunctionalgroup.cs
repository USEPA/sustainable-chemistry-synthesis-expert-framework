using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class AppFunctionalgroup
    {
        public AppFunctionalgroup()
        {
            AppNamedreaction = new HashSet<AppNamedreaction>();
            AppReference = new HashSet<AppReference>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Smarts { get; set; }
        public string Image { get; set; }

        public virtual ICollection<AppNamedreaction> AppNamedreaction { get; set; }
        public virtual ICollection<AppReference> AppReference { get; set; }
    }
}
