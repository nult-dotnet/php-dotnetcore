namespace BookStoreApi.ApiActionResult
{
    public class SuccessResult<T> : ApiResult<T>
    {
        public SuccessResult(int statuscode,string message,T obj)
        {
            StatusCode = statuscode;
            Message = message;
            Object = obj;
            IsSuccess = true;
        }
        public SuccessResult(int statuscode, string message)
        {
            StatusCode = statuscode;
            Message = message;
            IsSuccess = true;
        }
    }
}   