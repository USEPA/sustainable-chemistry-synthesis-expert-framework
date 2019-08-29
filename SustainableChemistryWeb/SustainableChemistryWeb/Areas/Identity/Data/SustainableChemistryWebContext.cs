using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SustainableChemistryWeb.Areas.Identity.Data;

namespace SustainableChemistryWeb.Models
{
    public class SustainableChemistryWebContext : IdentityDbContext<ApplicationUser>
    {
        public SustainableChemistryWebContext(DbContextOptions<SustainableChemistryWebContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
