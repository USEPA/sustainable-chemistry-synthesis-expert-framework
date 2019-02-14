using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class AppCatalyst
    {
        public AppCatalyst()
        {
            AppNamedreaction = new HashSet<AppNamedreaction>();
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AppNamedreaction> AppNamedreaction { get; set; }
    }
}
