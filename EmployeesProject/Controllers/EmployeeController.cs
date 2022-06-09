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

        /// <summary>
        /// Добавление сотрудника
        /// </summary>
        /// <param name="employee">Сотрудник</param>
        /// <returns>id добавленого сотрудника</returns>
        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            var result = await _employeeService.CreateEmployeeAsync(employee);
            return Ok($"{{\"id\":{result}}}");
        }

        /// <summary>
        /// Выводить список сотрудников для указанного отдела
        /// </summary>
        /// <param name="name">Название отдела</param>
        /// <returns>Список сотрудников</returns>
        [HttpGet]
        public async Task<IActionResult> GetByDepartment(string name)
        {
            var result = await _employeeService.GetByDepartmentAsync(name);
            return Ok(result);
        }

        /// <summary>
        /// Выводить список сотрудников для указанной компании
        /// </summary>
        /// <param name="companyId">id компании</param>
        /// <returns>Список сотрудников</returns>
        [HttpGet]
        public async Task<IActionResult> GetByCompany(int companyId)
        {
            var result = await _employeeService.GetByCompanyAsync(companyId);
            return Ok(result);
        }

        /// <summary>
        /// Изменение сотрудника
        /// </summary>
        /// <param name="employee">Обновленный сотрудник</param>
        /// <returns></returns>
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

        /// <summary>
        /// Удаление сотрудника
        /// </summary>
        /// <param name="id">id удаляемого сотрудника</param>
        /// <returns></returns>
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
