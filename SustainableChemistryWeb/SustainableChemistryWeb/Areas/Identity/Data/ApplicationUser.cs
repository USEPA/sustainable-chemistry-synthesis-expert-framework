using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SustainableChemistryWeb.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData] public string FirstName { get; set; }
        [PersonalData] public string LastName { get; set; }
        [PersonalData] public string Sector { get; set; }
        [PersonalData] public string Affiliation { get; set; }
        [PersonalData] public string JobTitle { get; set; }
        [PersonalData] public string Address1 { get; set; }
        [PersonalData] public string Address2 { get; set; }
        [PersonalData] public string City { get; set; }
        [PersonalData] public string State { get; set; }
        [PersonalData] public string ZipCode { get; set; }
        [PersonalData] public string Country { get; set; }

        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
        public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
        public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public virtual ApplicationUser User { get; set; }
    }

    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        public virtual ApplicationUser User { get; set; }
    }

    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationUserToken : IdentityUserToken<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
