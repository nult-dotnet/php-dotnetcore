namespace BookStoreApi.ApiActionResult
{
    public class ErrorResult<T> : ApiResult<T>
    {
        public ErrorResult(int statuscode,string message)
        {
            StatusCode = statuscode;
            Message = message;
            IsSuccess = false;
        }
    }
}