using EduGate.Core.Entities;
using EduGate.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core
{
    public interface IUnitOfWork : IAsyncDisposable 
    {

        IGenaricRepository<T> Repository<T>() where T : BaseEntity;

        Task<int> CompleteAsync();

    }
}
