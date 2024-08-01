namespace InternSystem.Application.Common.Constants
{
    public class ResponseCodeConstants
    {
        public const string NOT_FOUND = "Not found!";
        public const string SUCCESS = "Success!";
        public const string FAILED = "Failed!";
        public const string EXISTED = "Existed!";
        public const string DUPLICATE = "Duplicate!";
        public const string INTERNAL_SERVER_ERROR = "Internal server error!";
        public const string INVALID_INPUT = "Invalid input!";
        public const string UNAUTHORIZED = "Unauthorized!";
        public const string BADREQUEST = "Bad request!";
        public const string ERROR = "Error!";
    }

    public static class ErrorMessages
    {
        public const string NOT_FOUND = "Không tìm thấy {0}.";
        public const string SUCCESS = "Thành công!";
        public const string FAILED = "Thất bại!";
        public const string EXISTED = "{0} đã tồn tại.";
        public const string DUPLICATE = "{0} bị trùng lặp.";
        public const string INTERNAL_SERVER_ERROR = "Lỗi máy chủ nội bộ!";
        public const string INVALID_INPUT = "Dữ liệu đầu vào không hợp lệ!";
        public const string UNAUTHORIZED = "Không có quyền truy cập!";
        public const string BADREQUEST = "Yêu cầu không hợp lệ!";
        public const string ERROR = "Lỗi!";
    }
}
