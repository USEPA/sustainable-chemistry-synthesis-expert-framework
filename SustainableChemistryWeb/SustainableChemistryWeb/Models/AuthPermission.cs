using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class AuthPermission
    {
        public AuthPermission()
        {
            AuthGroupPermissions = new HashSet<AuthGroupPermissions>();
            AuthUserUserPermissions = new HashSet<AuthUserUserPermissions>();
        }

        public long Id { get; set; }
        public long ContentTypeId { get; set; }
        public string Codename { get; set; }
        public string Name { get; set; }

        public virtual DjangoContentType ContentType { get; set; }
        public virtual ICollection<AuthGroupPermissions> AuthGroupPermissions { get; set; }
        public virtual ICollection<AuthUserUserPermissions> AuthUserUserPermissions { get; set; }
    }
}
