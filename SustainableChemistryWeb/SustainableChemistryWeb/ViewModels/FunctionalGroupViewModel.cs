namespace SustainableChemistryWeb.ViewModels
{
    public class FunctionalGroupViewModel : SustainableChemistryWeb.Models.FunctionalGroup
    {
        [System.ComponentModel.DisplayName("Image File")]
        public Microsoft.AspNetCore.Http.IFormFile ImageFile { get; set; }
    }
}
