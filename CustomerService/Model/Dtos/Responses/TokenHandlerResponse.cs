namespace CustomerService.Model.Dtos.Responses
{
    public class TokenHandlerResponse
    {
        public string Id { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
        public string ValidTo { get; set; }
    }
}