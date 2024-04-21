using EduGate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Repositories.Contract
{
    public interface IDoctorRepository
    {
        Task<Doctor> GetbyName(string name);
    }
}
