using EduGate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Repositories.Contract
{
    public interface IAttendanceRepository
    {

        Task<Attendance> GetAttedance(int studentId, int courseId, int groupId, int lecNumber);
    }
}
