using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class Reference
    {
        public long Id { get; set; }
        public string Risdata { get; set; }
        public long? FunctionalGroupId { get; set; }
        public long? ReactionId { get; set; }

        public virtual FunctionalGroup FunctionalGroup { get; set; }
        public virtual NamedReaction Reaction { get; set; }
    }
}
