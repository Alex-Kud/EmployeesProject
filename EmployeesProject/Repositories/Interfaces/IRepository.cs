using EmployeesProject.Data.Entities;

namespace EmployeesProject.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<int> CreateAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(int id);
    }
}
