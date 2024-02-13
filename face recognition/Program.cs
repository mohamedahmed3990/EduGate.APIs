

namespace face_recognition
    
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:5000/predict");
            request.Headers.Add("Accept", "application/json");
            var content = new StringContent("{\r\n\r\n  \"img1\":  \"C:/Users/Expresss/Desktop/EduGate.APIs/EduGate.APIs/wwwroot/Upload/student/42020290.png\" ,\r\n  \"img2\" : \"C:/Users/Expresss/Desktop/EduGate.APIs/EduGate.APIs/wwwroot/Upload/student/333.png\"\r\n}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(response.StatusCode); 
            var status_code = response.StatusCode;
          
            Console.WriteLine($"{status_code}"); 
            Console.WriteLine(await response.Content.ReadAsStringAsync());

        }
    }
}