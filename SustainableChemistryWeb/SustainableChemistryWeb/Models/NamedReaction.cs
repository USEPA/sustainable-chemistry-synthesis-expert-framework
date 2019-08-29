using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class NamedReaction : UserIdStatus
    {
        public NamedReaction()
        {
            AppNamedreactionByProducts = new HashSet<NamedReactionByProducts>();
            AppNamedreactionReactants = new HashSet<NamedReactionReactants>();
            AppReference = new HashSet<Reference>();
        }

        public long Id { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }
        public string ReactantA { get; set; }
        public string ReactantB { get; set; }
        public string ReactantC { get; set; }
        public string Product { get; set; }
        public string Heat { get; set; }
        [System.ComponentModel.DisplayName("Acid/Base")]
        public string AcidBase { get; set; }
        public string Image { get; set; }
        public long CatalystId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public long FunctionalGroupId { get; set; }
        public long SolventId { get; set; }
        public string Url { get; set; }

        public virtual Catalyst Catalyst { get; set; }
        [System.ComponentModel.DisplayName("Functional Group")]
        public virtual FunctionalGroup FunctionalGroup { get; set; }
        public virtual Solvent Solvent { get; set; }
        public virtual ICollection<NamedReactionByProducts> AppNamedreactionByProducts { get; set; }
        public virtual ICollection<NamedReactionReactants> AppNamedreactionReactants { get; set; }
        public virtual ICollection<Reference> AppReference { get; set; }
    }
}
