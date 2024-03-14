using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Entities
{
    public class Attendance : BaseEntity
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int StudentCourseGroup { get; set; }
        public StudentCourseGroup studentCourseGroup { get; set; }
        public int LectureNumber { get; set; }
        public DateTime Date { get; set; }
    }
}
