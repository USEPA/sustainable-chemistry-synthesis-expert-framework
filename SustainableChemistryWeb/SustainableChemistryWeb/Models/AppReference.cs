using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class AppReference
    {
        public long Id { get; set; }
        public string Risdata { get; set; }
        public long? FunctionalGroupId { get; set; }
        public long? ReactionId { get; set; }

        public virtual AppFunctionalgroup FunctionalGroup { get; set; }
        public virtual AppNamedreaction Reaction { get; set; }
    }
}
