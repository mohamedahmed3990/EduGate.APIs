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
        public AttendanceSpecs(int studentId):base(a => a.StudentId == studentId && a.Attend == true)
        {
            InCludes.Add(a => a.Student);
            InCludes.Add(a => a.Course);
            InCludes.Add(a => a.Group);

        }

        public AttendanceSpecs(int studentId, int courseId, int groupId):base(a => a.StudentId == studentId && a.CourseId == courseId && a.GroupId == groupId)
        {
            
        }
        public AttendanceSpecs(int studentId, int courseId, int groupId, int lec):base(a => a.StudentId == studentId && a.CourseId == courseId && a.GroupId == groupId && a.LectureNumber == lec)
        {
            
        }

        public AttendanceSpecs(int courseId, int groupId) : base(a => a.CourseId == courseId && a.GroupId == groupId)
        {
            InCludes.Add(a => a.Student);

        }

    }
}
