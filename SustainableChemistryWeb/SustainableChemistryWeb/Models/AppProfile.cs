using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class AppProfile
    {
        public long Id { get; set; }
        public string Organization { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public long UserId { get; set; }

        public virtual AuthUser User { get; set; }
    }
}
