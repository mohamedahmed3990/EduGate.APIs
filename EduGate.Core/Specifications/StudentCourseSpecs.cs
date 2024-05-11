using EduGate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Specifications
{
    public class StudentCourseSpecs : BaseSpecification<StudentCourseGroup>
    {
        public StudentCourseSpecs() : base()
        {
            InCludes.Add(s => s.Student);
            InCludes.Add(s => s.Course);
            InCludes.Add(s => s.Group);
        }

        public StudentCourseSpecs(int studentId) : base(s => s.StudentId == studentId)
        {
            InCludes.Add(s => s.Course);
            InCludes.Add(s => s.Group);
        }

        public StudentCourseSpecs(int studentId, int courseId, int groupId) : base(s => s.StudentId == studentId && s.CourseId == courseId && s.GroupId == groupId)
        {
         
        }

        public StudentCourseSpecs(int studentId, int courseId) : base(s => s.StudentId == studentId && s.CourseId == courseId)
        {

        }
    }
}
