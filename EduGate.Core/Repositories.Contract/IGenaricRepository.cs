using EduGate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Repositories.Contract
{
    public interface IGenaricRepository<T> where T : BaseEntity 
    {
        Task<T?> GetByIdAsync(int id);

        Task<IReadOnlyList<T>>GetAllAsync();

        Task<T?> GetByIdWithSpecAsync(ISpecification<T> spec);

        Task<IReadOnlyList<T>>GetAllWithSpecAsync(ISpecification<T> spec); 
        Task<IReadOnlyList<T>>GetAllByIdWithSpecAsync(ISpecification<T> spec);





        Task AddAsync(T entity);

        void Update(T entity);
        void Delete(T entity);

        void DeleteAll(IEnumerable<T> entity);

    }
}
