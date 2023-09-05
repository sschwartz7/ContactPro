using ContactPro.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Npgsql;

namespace ContactPro.Data
{
    public static class DataUtility
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");//Project is in developement
            string? databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");// Project is Published
            return string.IsNullOrEmpty(databaseUrl) ? connectionString! : BuildConnectionString(databaseUrl);
        }
        private static string BuildConnectionString(string databaseUrl)
        {
            var databaseUri = new Uri(databaseUrl);
            var userInfo = databaseUri.UserInfo.Split(':');
            var builder = new NpgsqlConnectionStringBuilder()
            {
                Host = databaseUri.Host,
                Port = databaseUri.Port,
                Username = userInfo[0],
                Password = userInfo[1],
                Database = databaseUri.LocalPath.TrimStart('/'),
                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };
            return builder.ToString();
        }

        public static async Task ManageDataAsync(IServiceProvider svcProvider)
        {
            //Obtaining the necessary services based on the IServiceProvider parameter
            var dbContextSvc = svcProvider.GetRequiredService<ApplicationDbContext>();
            var userMangerSvc = svcProvider.GetRequiredService<UserManager<AppUser>>();
            var configurationSvc = svcProvider.GetRequiredService<IConfiguration>();

            //Align db by checking Migrations
            await dbContextSvc.Database.MigrateAsync();

            //Seed Demo Users
            await SeedDemoUsersAsync(userMangerSvc, configurationSvc);

        }

        // Demo Users Seed Method
        private static async Task SeedDemoUsersAsync(UserManager<AppUser> userManager, IConfiguration configuration)
        {
            string? demoLoginEmail = configuration["DemoLoginEmail"] ?? Environment.GetEnvironmentVariable("DemoLoginEmail");
            string? demoLoginPassword = configuration["DemoLoginPassword"] ?? Environment.GetEnvironmentVariable("DemoLoginPassword");

            AppUser demoUser = new AppUser()
            {
                UserName = demoLoginEmail,
                Email = demoLoginEmail,
                FirstName = "Aspen",
                LastName = "Angel",
                EmailConfirmed = true
            };
            try
            {
                AppUser? appUser = await userManager.FindByEmailAsync(demoLoginEmail!);

                if (appUser == null)
                {
                    await userManager.CreateAsync(demoUser, demoLoginPassword!);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("******************ERROR************");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("Error Seeding Demo Login User.");
                Console.WriteLine("***********************************");
                throw;
            }

        }
    }
}
