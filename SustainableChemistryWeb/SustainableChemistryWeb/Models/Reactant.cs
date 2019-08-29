using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class Reactant : UserIdStatus
    {
        public Reactant()
        {
            AppNamedreactionByProducts = new HashSet<NamedReactionByProducts>();
            AppNamedreactionReactants = new HashSet<NamedReactionReactants>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Temp2 { get; set; }

        public virtual ICollection<NamedReactionByProducts> AppNamedreactionByProducts { get; set; }
        public virtual ICollection<NamedReactionReactants> AppNamedreactionReactants { get; set; }
    }
}
