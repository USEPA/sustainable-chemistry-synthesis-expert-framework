using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SustainableChemistryWeb.ViewModels
{
    public class FunctionalGroupViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Smarts { get; set; }
        public Microsoft.AspNetCore.Http.IFormFile Image { get; set; }
        public string ImageFileName { get; set; }
    }
}
