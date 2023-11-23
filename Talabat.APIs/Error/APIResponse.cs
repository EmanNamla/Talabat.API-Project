namespace Talabat.APIs.Error
{
    public class APIResponse
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public APIResponse(int statusCode, string? message=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatuesCode(StatusCode);
        }

        private string GetDefaultMessageForStatuesCode(int statuesCode)
        {
            return StatusCode switch
            {
                400=>"Bad Request",
                401=>"You are Not Authorized",
                404=>"Resource Not Found",
                500=>"Internal Server Error",
                _=> null
            };
        }
    }
}
