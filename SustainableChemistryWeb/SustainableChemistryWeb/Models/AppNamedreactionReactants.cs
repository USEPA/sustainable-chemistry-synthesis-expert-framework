using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class AppNamedreactionReactants
    {
        public long Id { get; set; }
        public long NamedreactionId { get; set; }
        public long ReactantId { get; set; }

        public virtual AppNamedreaction Namedreaction { get; set; }
        public virtual AppReactant Reactant { get; set; }
    }
}
