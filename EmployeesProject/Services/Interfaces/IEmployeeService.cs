using EmployeesProject.Data.Entities;

namespace EmployeesProject.Services.Interfaces
{
    public interface IEmployeeService
    {
        public Task<List<Employee>> GetByDepartmentAsync(string name);
        public Task<List<Employee>> GetByCompanyAsync(int id);
        public Task<int> CreateEmployeeAsync(Employee employee);
        public Task<int> UpdateEmployeeAsync(Employee employee);
        public Task<int> DeleteEmployeeAsync(int id);
    }
}
