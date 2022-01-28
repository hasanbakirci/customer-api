namespace CustomerService.Model.Dtos.Responses
{
    public class AuthApiResponse
    {
        public TokenHandlerResponse Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}