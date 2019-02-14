using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class AuthUser
    {
        public AuthUser()
        {
            AuthUserGroups = new HashSet<AuthUserGroups>();
            AuthUserUserPermissions = new HashSet<AuthUserUserPermissions>();
            DjangoAdminLog = new HashSet<DjangoAdminLog>();
        }

        public long Id { get; set; }
        public string Password { get; set; }
        public string LastLogin { get; set; }
        public string IsSuperuser { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string IsStaff { get; set; }
        public string IsActive { get; set; }
        public string DateJoined { get; set; }
        public string LastName { get; set; }

        public virtual AppProfile AppProfile { get; set; }
        public virtual ICollection<AuthUserGroups> AuthUserGroups { get; set; }
        public virtual ICollection<AuthUserUserPermissions> AuthUserUserPermissions { get; set; }
        public virtual ICollection<DjangoAdminLog> DjangoAdminLog { get; set; }
    }
}
