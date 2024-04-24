using EduGate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Specifications
{
    public class AttendanceSpecs : BaseSpecification<Attendance>
    {
        public AttendanceSpecs(int studentId, int courseId, int groupId):base(a => a.StudentId == studentId && a.CourseId == courseId && a.GroupId == groupId)
        {
            
        }

        public AttendanceSpecs(int courseId, int groupId) : base(a => a.CourseId == courseId && a.GroupId == groupId)
        {
            InCludes.Add(a => a.Student);

        }

    }
}
