using EduGate.Core;
using EduGate.Core.Entities;
using EduGate.Core.Repositories.Contract;
using EduGate.Repositroy.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Repositroy
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        private Hashtable _repositories;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();

        }

        public IGenaricRepository<T> Repository<T>() where T : BaseEntity
        {

            var key = typeof(T).Name;
            if (!_repositories.ContainsKey(key))
            {
                var repository = new GenaricRepository<T>(_dbContext);
                _repositories.Add(key, repository); 

            }

            return _repositories[key] as IGenaricRepository<T>;


        }

        public async Task<int> CompleteAsync()
            => await _dbContext.SaveChangesAsync();
        

        public async ValueTask DisposeAsync()
            => await _dbContext.DisposeAsync();

    }
}
    