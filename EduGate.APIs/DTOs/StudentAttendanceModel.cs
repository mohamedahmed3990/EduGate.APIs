using System.ComponentModel.DataAnnotations;

namespace EduGate.APIs.DTOs
{
    public class StudentAttendanceModel
    {
        public int StudentId { get; set; }

        public int CourseId { get; set; }

        public int GroupId { get; set; }
    }
}
