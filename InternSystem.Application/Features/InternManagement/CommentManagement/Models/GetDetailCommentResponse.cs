namespace InternSystem.Application.Features.InternManagement.CommentManagement.Models
{
    public class GetDetailCommentResponse
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int IdNguoiDuocComment { get; set; }
        public string TenNguoiDuocComment { get; set; }
        public string IdNguoiComment { get; set; }
        public string TenNguoiComment { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LastUpdatedByName { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
