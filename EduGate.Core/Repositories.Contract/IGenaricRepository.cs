using EduGate.Core.Entities;
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
    }
}
