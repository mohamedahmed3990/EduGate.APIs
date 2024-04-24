namespace EduGate.APIs.DTOs
{
    public class AttendanceCourseForDoctorToReturnDto
    {
        public int DoctorId { get; set; }
        public IEnumerable<AttendanceCourse> Courses { get; set; }
}
}
