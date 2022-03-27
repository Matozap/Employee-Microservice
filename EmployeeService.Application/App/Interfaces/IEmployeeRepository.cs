using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmployeeService.Domain.EmployeeDemographics;

namespace EmployeeService.Application.App.Interfaces
{
    public interface IEmployeeRepository
    {
        IQueryable<Employee> GetAll();

        Task<List<Employee>> GetAllAsync(int from, int to);

        Task<Employee> Get(Expression<Func<Employee, bool>> predicate);

        Task<List<Employee>> GetAsList(Expression<Func<Employee, bool>> predicate);

        Task<Employee> AddAsync(Employee entity);

        Task<Employee> UpdateAsync(Employee entity);

        Task<Employee> DeleteAsync(Employee entity);
    }
}