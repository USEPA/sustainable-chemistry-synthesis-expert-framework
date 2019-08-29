using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class Solvent : UserIdStatus
    {
        public Solvent()
        {
            AppNamedreaction = new HashSet<NamedReaction>();
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public virtual ICollection<NamedReaction> AppNamedreaction { get; set; }
    }
}
