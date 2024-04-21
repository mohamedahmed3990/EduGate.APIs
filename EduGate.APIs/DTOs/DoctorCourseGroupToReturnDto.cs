namespace EduGate.APIs.DTOs
{
    public class DoctorCourseGroupToReturnDto
    {
        public int DoctorId { get; set; }

        public string DoctorName { get; set; }
        public int CourseId { get; set; }

        public string CourseName { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
