using EmployeesProject.Migrations;
using FluentMigrator.Runner;

namespace EmployeesProject.Data
{
    public static class MigrationManager
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
                var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                try
                {
                    databaseService.CreateDatabase("Employees");

                    migrationService.ListMigrations();
                    migrationService.MigrateUp();
                }
                catch
                {
                    Console.WriteLine("Something went wrong. The database cannot be created...");
                    throw;
                }
            }
            return host;
        }
    }
}
