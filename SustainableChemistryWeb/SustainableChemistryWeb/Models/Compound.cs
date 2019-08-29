namespace SustainableChemistryWeb.Models
{
    public partial class Compound : UserIdStatus
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string CasNumber { get; set; }
        public string Name { get; set; }
    }
}
