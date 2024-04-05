using EduGate.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Entities
{
    public class DoctorCourseGroup : BaseEntity
    {
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int CourseGroupId { get; set; }
        public CourseGroup CourseGroup { get; set; }
    }
}
