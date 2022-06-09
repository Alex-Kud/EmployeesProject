using EmployeesProject.Data.Entities;
using EmployeesProject.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesProject.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            var result = await _employeeService.CreateEmployeeAsync(employee);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByDepartment(string name)
        {
            var result = await _employeeService.GetByDepartmentAsync(name);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCompany(int companyId)
        {
            var result = await _employeeService.GetByCompanyAsync(companyId);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Change(Employee employee)
        {
            int result = await _employeeService.UpdateEmployeeAsync(employee);
            if (result != -1)
            {
                return Ok();
            }
            else
            {
                return UnprocessableEntity();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            int result = await _employeeService.DeleteEmployeeAsync(id);
            if (result != -1)
            {
                return Ok();
            }
            else
            {
                return UnprocessableEntity();
            }
        }
    }
}
