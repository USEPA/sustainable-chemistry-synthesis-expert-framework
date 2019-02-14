using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class AppCompound
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public string CasNumber { get; set; }
        public string Name { get; set; }
    }
}
