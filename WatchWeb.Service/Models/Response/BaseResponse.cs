namespace WatchWeb.Service.Models.Response
{
    public class BaseResponse<TData>
    {
        public BaseResponse()
        {

        }

        public BaseResponse<TData> Success(string message, TData data)
        {
            return new BaseResponse<TData>
            {
                Message = message,
                IsSuccess = true,
                Data = data
            };
        }

        public BaseResponse<TData> Failed(string message, TData data)
        {
            return new BaseResponse<TData>
            {
                Message = message,
                IsSuccess = false,
                Data = data
            };
        }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public TData Data { get; set; }
    }
}
