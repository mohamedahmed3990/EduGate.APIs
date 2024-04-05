using EduGate.Core.Entities;
using EduGate.Core.Repositories.Contract;
using EduGate.Repositroy.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Repositroy
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _dbContext;

        public GenaricRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Add(T entity)
        {
             _dbContext.Add(entity);
            return _dbContext.SaveChanges();
        }

        public int Delete(T entity)
        {
            _dbContext.Remove(entity);
            return _dbContext.SaveChanges();
        }

        public int Update(T entity)
        {
            _dbContext.Update(entity);
            return _dbContext.SaveChanges();
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }


        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

    }
}
