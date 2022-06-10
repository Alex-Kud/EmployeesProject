using Dapper;
using EmployeesProject.Data;
using EmployeesProject.Data.Entities;
using EmployeesProject.Repositories.Interfaces;

namespace EmployeesProject.Repositories.Implimentations
{
    public class EmployeeRepository : IRepository<Employee>, IEmployeeRepository
    {
        private readonly DapperContext _context;

        public EmployeeRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> CreateAsync(Employee entity)
        {
            try
            {
                using var connection = _context.CreateConnection();
                int passportId = entity.PassportId ?? 0,
                departmentId = entity.DepartmentId ?? 0;

                var checkExistPassport = "SELECT COUNT(*) FROM Passports WHERE Id = @id;";
                if (entity.Passport != null && await connection.QueryFirstAsync<int>(checkExistPassport, new {id = entity.PassportId }) == 0)
                {
                    var insertPassport = "INSERT INTO Passports (Type, Number) OUTPUT INSERTED.Id VALUES (@Type, @Number);";
                    passportId = await connection.QueryFirstAsync<int>(insertPassport, new
                    {
                        Type = entity.Passport.Type,
                        Number = entity.Passport.Number
                    });
                }

                var checkExistDepartment = "SELECT COUNT(*) FROM Departments WHERE Id = @id;";
                if (entity.Department != null && await connection.QueryFirstAsync<int>(checkExistDepartment, new { id = entity.PassportId }) == 0)
                {
                    var insertDepartment = "INSERT INTO Departments (Name, Phone) OUTPUT INSERTED.Id VALUES (@Name, @Phone);";
                    departmentId = await connection.QueryFirstAsync<int>(insertDepartment, new
                    {
                        Name = entity.Department.Name,
                        Phone = entity.Department.Phone
                    });
                }

                var insertEmployee = "INSERT INTO Employees (Name, Surname, Phone, CompanyId, DepartmentId, PassportId) " +
                    "OUTPUT INSERTED.Id VALUES (@Name, @Surname, @Phone, @CompanyId, @DepartmentId, @PassportId);";

                var id = await connection.QueryFirstAsync<int>(insertEmployee, new
                {
                    Name = entity.Name,
                    Surname = entity.Surname,
                    Phone = entity.Phone,
                    CompanyId = entity.CompanyId,
                    DepartmentId = departmentId,
                    PassportId = passportId
                });
                return Convert.ToInt32(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var checkExistEmployee = "SELECT COUNT(*) FROM Employees WHERE Id = @id;";
                if (await connection.QueryFirstAsync<int>(checkExistEmployee, new { id = id }) == 0)
                {
                    return -1;
                }

                var deleteEmployee = "DELETE FROM Employees WHERE Id = @Id;";
                var getPassportId = "SELECT PassportId FROM Employees WHERE Id = @Id;";
                var passportId = await connection.QueryAsync<int>(getPassportId, new { Id = id });

                var result = await connection.ExecuteAsync(deleteEmployee, new { Id = id });

                if (passportId != null)
                {
                    var deletePassport = "DELETE FROM Passports WHERE Id = @Id;";
                    await connection.ExecuteAsync(deletePassport, new { Id = passportId });
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Employee>> GetByCompanyAsync(int companyId)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var getByCompanyId = "SELECT * FROM Employees " +
                    "JOIN Departments dep ON Employees.DepartmentId = dep.Id " +
                    "JOIN Passports pass ON pass.Id = Employees.PassportId " +
                    "WHERE Employees.CompanyId = @CompanyId;";

                var result = await connection.QueryAsync<Employee, Department, Passport, Employee>(getByCompanyId,
                    (employee, department, passport) =>
                    {
                        employee.Passport = passport;
                        employee.Department = department;

                        return employee;
                    }, new { CompanyId = companyId });
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Employee>> GetByDepartmentAsync(string name)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var getByNameDep = "SELECT * FROM Employees " +
                    "JOIN Departments dep ON Employees.DepartmentId = dep.Id " +
                    "JOIN Passports pass ON pass.Id = Employees.PassportId " +
                    "WHERE dep.Name = @DepName;";
                
                var result = await connection.QueryAsync<Employee, Department, Passport, Employee>(getByNameDep,
                    (employee, department, passport) =>
                    {
                        employee.Passport = passport;
                        employee.Department = department;

                        return employee;
                    }, new { DepName = name});
                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> UpdateAsync(Employee entity)
        {
            try
            {
                using var connection = _context.CreateConnection();

                var updateEmployee = "UPDATE Employees SET " +
                    "Name = COALESCE(@Name,Name), " +
                    "Surname = COALESCE(@Surname, Surname), " +
                    "Phone = COALESCE(@Phone, Phone), " +
                    "CompanyId = COALESCE(@CompanyId, CompanyId), " +
                    "DepartmentId = COALESCE(@DepartmentId, DepartmentId), " +
                    "PassportId = COALESCE(@PassportId, PassportId) " +
                    "WHERE Id = @Id;";

                var updatePassport = "UPDATE Passports SET " +
                    "Number = COALESCE(@Number, Number), " +
                    "Type = COALESCE(@Type, Type) " +
                    "WHERE Id = @PassportId;";

                var updateDepartment = "UPDATE Departments SET " +
                    "Name = COALESCE(@Name, Name), " +
                    "Phone = COALESCE(@Phone, Phone) " +
                    "WHERE Id = @DepartmentId;";

                var checkExistEmployee = "SELECT COUNT(*) FROM Employees WHERE Id = @id;";
                var checkExistPassport = "SELECT COUNT(*) FROM Passports WHERE Id = @id;";
                var checkExistDepartment = "SELECT COUNT(*) FROM Departments WHERE Id = @id;";        

                if (await connection.QueryFirstAsync<int>(checkExistEmployee, new { id = entity.Id }) == 0 ||
                    await connection.QueryFirstAsync<int>(checkExistPassport, new { id = entity.PassportId }) == 0 ||
                    await connection.QueryFirstAsync<int>(checkExistDepartment, new { id = entity.DepartmentId }) == 0)
                {
                    return -1;
                }

                if (entity.Department != null)
                {
                    await connection.ExecuteAsync(updateDepartment, new
                    {
                        DepartmentId = entity.Department.Id,
                        Name = entity.Department.Name,
                        Phone = entity.Department.Phone
                    });
                }

                if (entity.Passport != null)
                {
                    await connection.ExecuteAsync(updatePassport, new 
                    {
                        PassportId = entity.Passport.Id,
                        Type = entity.Passport.Type,
                        Number = entity.Passport.Number
                    });
                }

                return await connection.ExecuteAsync(updateEmployee, new
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Surname = entity.Surname,
                    Phone = entity.Phone,
                    DepartmentId = entity.DepartmentId,
                    PassportId = entity.PassportId,
                    CompanyId = entity.CompanyId
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
