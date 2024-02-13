namespace EduGate.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }

        public string? ErrorMessage { get; set; }

        public ApiResponse(int _statusCode, string? _errorMessage = null)
        {
            StatusCode = _statusCode;
            ErrorMessage = _errorMessage ?? GetDefaultMessage(StatusCode);
        }

        private string? GetDefaultMessage(int? statusCode)
        {
            return statusCode switch
            {
                500 => "Internal Server Error",
                400 => "Bad Request",
                401 => "You are not Authoritzed",
                404 => "Not Found",
                _ => null
            };

        }
    }
}
