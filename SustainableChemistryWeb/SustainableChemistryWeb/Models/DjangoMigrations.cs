using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class DjangoMigrations
    {
        public long Id { get; set; }
        public string App { get; set; }
        public string Name { get; set; }
        public string Applied { get; set; }
    }
}
