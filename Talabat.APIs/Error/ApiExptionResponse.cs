namespace Talabat.APIs.Error
{
    public class ApiExptionResponse:APIResponse
    {
        public string? Details { get; set; }
        public ApiExptionResponse(int statusCode,string? message=null,string? details = null):base(statusCode, message)
        {
            Details = details;
        }
    }
}
