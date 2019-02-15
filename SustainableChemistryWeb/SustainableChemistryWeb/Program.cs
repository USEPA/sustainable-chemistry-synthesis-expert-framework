using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SustainableChemistryWeb
{
    // Scaffolding the database:
    // Open Nuget Package Manager Console
    // run: Scaffold-DbContext "DataSource= ..\\..\\SustainableChemistryData\\SustainableChemistryData\\SustainableChemistry.sqlite3" Microsoft.EntityFrameworkCore.Sqlite -OutputDir Models -Force

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
