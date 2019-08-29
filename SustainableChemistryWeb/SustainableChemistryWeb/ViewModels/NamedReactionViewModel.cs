namespace SustainableChemistryWeb.ViewModels
{
    public class NamedReactionViewModel : SustainableChemistryWeb.Models.NamedReaction
    {
        [System.ComponentModel.DisplayName("Image File")]
        public Microsoft.AspNetCore.Http.IFormFile ImageFile { get; set; }
    }
}
