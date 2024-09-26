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

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
