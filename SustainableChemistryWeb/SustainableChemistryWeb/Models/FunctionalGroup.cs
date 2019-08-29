using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SustainableChemistryWeb.Models
{
    public partial class FunctionalGroup : UserIdStatus
    {
        public FunctionalGroup()
        {
            AppNamedreaction = new HashSet<NamedReaction>();
            AppReference = new HashSet<Reference>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [System.ComponentModel.DisplayName("SMARTS")]
        [Required]
        public string Smarts { get; set; }
        public string Image { get; set; }
        public string URL { get; set; }

        public virtual ICollection<NamedReaction> AppNamedreaction { get; set; }
        public virtual ICollection<Reference> AppReference { get; set; }
    }
}
