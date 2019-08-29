namespace SustainableChemistryWeb.Models
{
    public partial class NamedReactionByProducts
    {
        public long Id { get; set; }
        public long NamedreactionId { get; set; }
        public long ReactantId { get; set; }

        public virtual NamedReaction Namedreaction { get; set; }
        public virtual Reactant Reactant { get; set; }
    }
}
