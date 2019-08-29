using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class Catalyst : UserIdStatus
    {
        public Catalyst()
        {
            AppNamedreaction = new HashSet<NamedReaction>();
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public virtual ICollection<NamedReaction> AppNamedreaction { get; set; }
    }
}
