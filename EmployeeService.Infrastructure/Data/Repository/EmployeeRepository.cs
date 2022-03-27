using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using EmployeeService.Application.App.Interfaces;
using EmployeeService.Domain.EmployeeDemographics;
using EmployeeService.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Infrastructure.Data.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeContext _applicationContext;

        public EmployeeRepository(EmployeeContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public IQueryable<Employee> GetAll()
        {
            return _applicationContext.Set<Employee>().AsNoTracking();
        }

        public async Task<List<Employee>> GetAllAsync(int from, int to)
        {
            var result = _applicationContext.Set<Employee>()
                .OrderBy(e => e.Id)
                .Skip(from)
                .Take(to);
            return await LoadAllNavigationalProperties(result).ToListAsync();
        }

        public async Task<Employee> Get(Expression<Func<Employee, bool>> predicate)
        {
            var result = _applicationContext.Set<Employee>()
                .Where(predicate);

            return await LoadAllNavigationalProperties(result).FirstOrDefaultAsync();
        }

        public async Task<List<Employee>> GetAsList(Expression<Func<Employee, bool>> predicate)
        {
            var result = _applicationContext.Set<Employee>().Where(predicate);

            return await LoadAllNavigationalProperties(result).ToListAsync();
        }

        public async Task<Employee> AddAsync(Employee entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Employee)} entity must not be null");
            }

            await _applicationContext.AddAsync(entity);
            await _applicationContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Employee> UpdateAsync(Employee entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Employee)} entity must not be null");
            }

            _applicationContext.Update(entity);
            await _applicationContext.SaveChangesAsync();

            return entity;
        }

        public async Task<Employee> DeleteAsync(Employee entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Employee)} entity must not be null");
            }

            _applicationContext.Remove(entity);
            await _applicationContext.SaveChangesAsync();

            return entity;
        }

        private static IQueryable<Employee> LoadAllNavigationalProperties(IQueryable<Employee> source)
        {
            return source.Include(e => e.HomeCity).ThenInclude(e => e.State).ThenInclude(e => e.Country)
                .Include(e => e.BirthCity).ThenInclude(e => e.State).ThenInclude(e => e.Country)
                .AsNoTracking();
        }
    }
}