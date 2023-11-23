namespace Talabat.APIs.Error
{
    public class ApiValidationErrorResponse:APIResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationErrorResponse():base(400)
        {
            Errors = new List<string>();
        }

    }
}
