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
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly AppDbContext _context;

        public AttendanceRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Attendance> GetAttedance(int studentId, int courseId, int groupId, int lecNumber)
        {
            return await _context.Set<Attendance>().FirstOrDefaultAsync(a => a.StudentId == studentId && a.CourseId == courseId && a.GroupId == groupId && a.LectureNumber == lecNumber);
        }
    }
}
