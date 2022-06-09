namespace EmployeesProject.Data
{
    public static class DbManager
    {
        public static IHost DatabaseManager(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var databaseService = scope.ServiceProvider.GetRequiredService<DapperContext>();
                try
                {
                    databaseService.CreateDatabase("Employees");
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
