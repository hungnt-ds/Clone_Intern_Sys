using System.ComponentModel.DataAnnotations;

namespace InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models
{
    public class NotificationTask
    {
        public int TaskId { get; set; }

        [Required(ErrorMessage = "ID người nhận là bắt buộc.")]
        public string IdNguoiNhan { get; set; }

        [Required(ErrorMessage = "ID người gửi là bắt buộc.")]
        public string IdNguoiGui { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được rỗng.")]
        public string TieuDe { get; set; }

        [Required(ErrorMessage = "Nội dung không được rỗng.")]
        public string NoiDung { get; set; }

        [Required(ErrorMessage = "Email của intern không được rỗng.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string InternEmail { get; set; }

        public string Deadline { get; set; }
    }
}
