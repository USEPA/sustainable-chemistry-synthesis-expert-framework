using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SustainableChemistryWeb.ViewModels
{
    public class ReferenceCreateViewModel: SustainableChemistryWeb.Models.Reference
    {
        public Microsoft.AspNetCore.Http.IFormFile RISFile { get; set; }
    }
}
