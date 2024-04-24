namespace EduGate.APIs.DTOs
{
    public class AttendanceToReturnDto
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }

        public List<bool> StudentAttend {  get; set; }
    }
}
