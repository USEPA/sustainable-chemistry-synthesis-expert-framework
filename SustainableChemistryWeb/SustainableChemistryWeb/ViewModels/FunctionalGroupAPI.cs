namespace SustainableChemistryWeb.ViewModels
{
    public class FunctionalGroupAPI
    {
        public long Id { get; set; }
        public string Name { get; set; }
        [System.ComponentModel.DisplayName("SMARTS")]
        public string Smarts { get; set; }
        public string Image { get; set; }
        public string URL { get; set; }
    }
}
