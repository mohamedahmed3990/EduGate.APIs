using EduGate.APIs.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace EduGate.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceRecognitionController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _configuration;

        public FaceRecognitionController(IHttpClientFactory clientFactory, IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _webHostEnvironment = webHostEnvironment;
           _configuration = configuration;
        }


        [HttpPost]
        public async Task<ActionResult<string>> CompareImage(ImageModel model)
        {
            string path = $"{_configuration["ImagePath"]}";

            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_configuration["ModelUrl"]}/predict");
            request.Headers.Add("Accept", "application/json");


            var content = new StringContent($"{{ \"img1\": \"{path}{model.ImageRegisterPath}\", \"img2\": \"{path}{model.ImageScanPath}\"}}", null, "application/json");
            request.Content = content;

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();

            return Ok(result);
        }
    }
}
