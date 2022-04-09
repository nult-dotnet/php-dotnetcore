namespace BookStoreApi.ApiActionResult
{
    public class ApiResult<T>
    {
        public bool IsSuccess { get; set; }

        public T Object { get; set; }

        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}