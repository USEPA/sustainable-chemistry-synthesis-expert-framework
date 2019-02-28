using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SustainableChemistryWeb.Areas.Identity.Data;
using SustainableChemistryWeb.Models;

[assembly: HostingStartup(typeof(SustainableChemistryWeb.Areas.Identity.IdentityHostingStartup))]
namespace SustainableChemistryWeb.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SustainableChemistryWebContext>(options =>
                    options.UseSqlite(
                        context.Configuration["Production:SqliteConnectionString"]));

                services.AddDefaultIdentity<SustainableChemistryWebUser>()
                    .AddEntityFrameworkStores<SustainableChemistryWebContext>();
            });
        }
    }
}