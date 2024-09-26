
using EduGate.Core.Entities;
using EduGate.Core.Repositories.Contract;
using EduGate.Repositroy.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Repositroy.Repositroies
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Doctor> GetbyName(string name)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.Name == name);
        }
        
        public async Task<Doctor> GetbyUserId(string id)
        {
            return await _context.Doctors.FirstOrDefaultAsync(d => d.UserId == id);
        }
    }
}
