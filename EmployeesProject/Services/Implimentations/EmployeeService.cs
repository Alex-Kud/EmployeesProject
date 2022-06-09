using EmployeesProject.Data.Entities;
using EmployeesProject.Repositories.Interfaces;
using EmployeesProject.Services.Interfaces;

namespace EmployeesProject.Services.Implimentations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<int> CreateEmployeeAsync(Employee employee)
        {
            return await _employeeRepository.CreateAsync(employee);
        }

        public async Task<int> DeleteEmployeeAsync(int id)
        {
            return await _employeeRepository.DeleteAsync(id);
        }

        public async Task<List<Employee>> GetByDepartmentAsync(string name)
        {
            return await _employeeRepository.GetByDepartmentAsync(name);
        }

        public async Task<List<Employee>> GetByCompanyAsync(int companyId)
        {
            return await _employeeRepository.GetByCompanyAsync(companyId);
        }

        public async Task<int> UpdateEmployeeAsync(Employee employee)
        {
            return await _employeeRepository.UpdateAsync(employee);
        }
    }
}
