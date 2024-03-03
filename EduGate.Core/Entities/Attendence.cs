using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core.Entities
{
    public class Attendence
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int LectureNumber { get; set; }
        public DateTime AttendanceDate { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
