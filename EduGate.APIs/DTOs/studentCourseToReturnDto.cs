namespace EduGate.APIs.DTOs
{
    public class studentCourseToReturnDto
    {
        public int StudentId { get; set; }

        public string StudentName { get; set; }
        public int CourseId { get; set; }

        public string CourseName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
