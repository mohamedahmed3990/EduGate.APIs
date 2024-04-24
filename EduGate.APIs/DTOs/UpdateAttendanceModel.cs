namespace EduGate.APIs.DTOs
{
    public class UpdateAttendanceModel
    {       
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int GroupId { get; set; }
        public int LectureNumber { get; set; }
        public bool Attend { get; set; }
    }
}

