using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SustainableChemistryWeb.Models
{
    public partial class FunctionalGroup
    {
        public FunctionalGroup()
        {
            AppNamedreaction = new HashSet<NamedReaction>();
            AppReference = new HashSet<Reference>();
        }

        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Smarts { get; set; }
        public string Image { get; set; }

        public virtual ICollection<NamedReaction> AppNamedreaction { get; set; }
        public virtual ICollection<Reference> AppReference { get; set; }
    }
}
