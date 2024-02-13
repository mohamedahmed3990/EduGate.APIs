namespace EduGate.APIs.DTOs
{
    public class UploadDto
    {
        public string FileName { get; set; }

        public IFormFile File { get; set; }
    }
    
}
