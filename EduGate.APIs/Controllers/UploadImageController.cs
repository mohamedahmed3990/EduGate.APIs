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


        [HttpPut("uploadfile")]
        public async Task<ActionResult<ImagePathReturn>> UploadImage(IFormFile formFile, string studentId)
        {
            try
            {
                string Filepath = $"{_webHostEnvironment.WebRootPath}/Upload/student";
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }


                string imagepath = $"{Filepath}/{studentId}.png";
                if (System.IO.File.Exists(imagepath))
                {
                    System.IO.File.Delete(imagepath);
                }

                using (FileStream stream = System.IO.File.Create(imagepath))
                {
                    await formFile.CopyToAsync(stream);
                }

            }
            catch (Exception ex)
            {
                ApiResponse apiResponse = new ApiResponse(100, ex.Message);

            }

            var user = await _userManager.FindByNameAsync(studentId);
            if (user is null) return BadRequest(new ApiResponse(400));

            ImagePathReturn imageReturn = new ImagePathReturn()
            {
                ImageUrl = user.PictureUrl,
                Message = "Image Uploaded Successfully"
            };

            return Ok(imageReturn);
            
        }

        [Authorize]
        [HttpGet("GetImage")]
        public async Task<ActionResult<ImagePathReturn>> GetImage(string studentId)
        {
            string Imageurl = string.Empty;
            try
            {
                string Filepath = $"{_webHostEnvironment.WebRootPath}/Upload/student/";
                string imagepath = $"{Filepath}/{studentId}.png";
                if (System.IO.File.Exists(imagepath))
                {
                    Imageurl = $"{_configuration["BaseUrl"]}/Upload/student/{studentId}.png";
                }
                else
                {
                    return NotFound(new ApiResponse(404));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiExceptionResponse(500, ex.Message));
            }

            return Ok(new ImagePathReturn()
            {
                ImageUrl = Imageurl,
                Message = "Image Returned Successfully"
            });
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

