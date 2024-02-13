using EduGate.APIs.DTOs;
using EduGate.APIs.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduGate.APIs.Controllers
{
    public class UploadImageController : BaseApiController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public UploadImageController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
           _configuration = configuration;
        }


        [HttpPut("uploadfile")]
        public async Task<ActionResult> UploadImage(IFormFile formFile, string studentId)
        {
            try
            {
                string Filepath = _webHostEnvironment.WebRootPath + "\\Upload\\student\\";
                if (!System.IO.Directory.Exists(Filepath))
                {
                    System.IO.Directory.CreateDirectory(Filepath);
                }

                string imagepath = Filepath + "\\" + studentId + ".png";
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

            return Ok("Image Saved Successfully");
        }

        [HttpGet("GetImage")]
        public async Task<IActionResult> GetImage(string studentId)
        {
            string Imageurl = string.Empty;
            try
            {
                string Filepath = $"{_webHostEnvironment.WebRootPath}/Upload/student/";
                string imagepath = Filepath + "\\" + studentId + ".png";
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
            }
            return Ok(Imageurl);
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

