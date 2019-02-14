using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class DjangoContentType
    {
        public DjangoContentType()
        {
            AuthPermission = new HashSet<AuthPermission>();
            DjangoAdminLog = new HashSet<DjangoAdminLog>();
        }

        public long Id { get; set; }
        public string AppLabel { get; set; }
        public string Model { get; set; }

        public virtual ICollection<AuthPermission> AuthPermission { get; set; }
        public virtual ICollection<DjangoAdminLog> DjangoAdminLog { get; set; }
    }
}
