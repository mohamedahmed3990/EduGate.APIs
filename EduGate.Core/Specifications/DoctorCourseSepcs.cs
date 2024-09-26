using EduGate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Specifications
{
    public class DoctorCourseSepcs : BaseSpecification<DoctorCourseGroup>
    {
        public DoctorCourseSepcs() : base()
        {
            InCludes.Add(d => d.Doctor);
            InCludes.Add(d => d.Course);
            InCludes.Add(d => d.Group);
        }

        public DoctorCourseSepcs(int doctorId) : base(d => d.DoctorId == doctorId)
        {
            InCludes.Add(d => d.Course);
            InCludes.Add(d => d.Group);
        }

        public DoctorCourseSepcs(int doctorId, int courseId, int groupId) : base(d => d.DoctorId == doctorId && d.CourseId == courseId && d.GroupId == groupId)
        {

        }
    }
}
