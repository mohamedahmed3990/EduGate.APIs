using EduGate.APIs.DTOs;
using EduGate.APIs.Errors;
using EduGate.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduGate.APIs.Controllers
{
    public class UploadImageController : BaseApiController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;
        private readonly UserManager<AppUser> _userManager;

        public UploadImageController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration, UserManager<AppUser> userManager)
        {
            _webHostEnvironment = webHostEnvironment;
           _configuration = configuration;
            _userManager = userManager;
        }



        [HttpPost("uploadfile")]
        public async Task<ActionResult<ImagePathReturn>> UploadImage(IFormFile file, string studentId)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new ApiResponse(400, "No file uploaded"));
            }

            //1. get location folder path
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Upload\\student");

            //2. get file name
            string fileName = $"{studentId}.png";

            //3. get file path
            string filePath = Path.Combine(folderPath, fileName);

            //4. save file with stream
            var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);

            try
            {
                // Convert uploaded image to base64 string
                byte[] imageBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }

                string base64String = Convert.ToBase64String(imageBytes);
                return Ok(new ImagePathReturn
                {
                    ImageUrl = base64String,
                    Message = "Image uploaded successfully :)"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet("getImage")]
        public async Task<ActionResult<ImagePathReturn>> GetImage(string studentId)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Upload\\student");
            string fileName = $"{studentId}.png";
            string filePath = Path.Combine(folderPath, fileName);

            try
            {
                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new ApiResponse(404, "image not found"));
                }

                byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);

                string base64String = Convert.ToBase64String(imageBytes);

                return Ok(new ImagePathReturn
                {
                    ImageUrl = base64String,
                    Message = "Image retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}








/*var options = new RestClientOptions("http://127.0.0.1:5000")
{
    MaxTimeout = -1,
};
var client = new RestClient(options);
var request = new RestRequest("/predict", Method.Post);
request.AddHeader("Accept", "application/json");
request.AddHeader("Content-Type", "application/json");
var body = @"{
" + "\n" +
@"
" + "\n" +
@"  ""img1"":  ""E:1.jpg"" ,
" + "\n" +
@"  ""img2"" : ""E:/2.jpeg""
" + "\n" +
@"}";
request.AddStringBody(body, DataFormat.Json);
RestResponse response = await client.ExecuteAsync(request);
Console.WriteLine(response.Content);*/

