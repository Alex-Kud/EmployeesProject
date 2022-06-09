using EmployeesProject.Data.Entities;

namespace EmployeesProject.Repositories.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        public Task<List<Employee>> GetByDepartmentAsync(string name);
        public Task<List<Employee>> GetByCompanyAsync(int companyId);
    }
}
