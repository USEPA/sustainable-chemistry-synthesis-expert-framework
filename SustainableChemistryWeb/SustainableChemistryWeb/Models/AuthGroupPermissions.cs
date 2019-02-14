using System;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Models
{
    public partial class AuthGroupPermissions
    {
        public long Id { get; set; }
        public long GroupId { get; set; }
        public long PermissionId { get; set; }

        public virtual AuthGroup Group { get; set; }
        public virtual AuthPermission Permission { get; set; }
    }
}
