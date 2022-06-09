using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EmployeesProject.Data
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_configuration.GetConnectionString("SqlConnection"));

        public void CreateDatabase(string dbName)
        {
            var query = "SELECT * FROM sys.databases WHERE name = @name";
            var parameters = new DynamicParameters();
            parameters.Add("name", dbName);
            using (var connection = CreateConnection())
            {
                var records = connection.Query(query, parameters);
                if (!records.Any())
                {
                    var initDB = $"CREATE DATABASE {dbName}" +

                    "CREATE TABLE[dbo].[Passports] ([Id] INT NOT NULL IDENTITY(1,1), " +
                    "[Type] NVARCHAR(255) NOT NULL, [Number] NVARCHAR(255) NOT NULL, CONSTRAINT[PK_Passports] PRIMARY KEY([Id]));" +

                    "CREATE TABLE[dbo].[Departments] ([Id] INT NOT NULL IDENTITY(1,1), [Name] NVARCHAR(255) " +
                    "NOT NULL, [Phone] NVARCHAR(255) NOT NULL, CONSTRAINT[PK_Departments] PRIMARY KEY([Id]));" +

                    "CREATE TABLE[dbo].[Employees] ([Id] INT NOT NULL IDENTITY(1,1), [Name] NVARCHAR(255), " +
                    "[Surname] NVARCHAR(255), [Phone] NVARCHAR(255), [CompanyId] INT, [PassportId] INT, " +
                    "[DepartmentId] INT, CONSTRAINT[PK_Employees] PRIMARY KEY([Id]));" +

                    "ALTER TABLE[dbo].[Employees] ADD CONSTRAINT[FK_Employees_PassportId_Passports_Id] " +
                    "FOREIGN KEY([PassportId]) REFERENCES[dbo].[Passports]([Id]);" +

                    "ALTER TABLE[dbo].[Employees] ADD CONSTRAINT[FK_Employees_DepartmentId_Departments_Id] " +
                    "FOREIGN KEY([DepartmentId]) REFERENCES[dbo].[Departments]([Id]);";

                    connection.Execute(initDB);
                }
            }
        }
    }
}
