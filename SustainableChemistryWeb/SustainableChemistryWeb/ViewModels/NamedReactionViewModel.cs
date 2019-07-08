using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 

namespace SustainableChemistryWeb.ViewModels
{
    public class NamedReactionViewModel: SustainableChemistryWeb.Models.NamedReaction
    {
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DisplayName("Image File")]
        public Microsoft.AspNetCore.Http.IFormFile ImageFile { get; set; }
    }
}
