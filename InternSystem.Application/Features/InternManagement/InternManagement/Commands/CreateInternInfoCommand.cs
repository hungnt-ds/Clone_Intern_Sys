using FluentValidation;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Commands
{
    public class CreateInternInfoValidator : AbstractValidator<CreateInternInfoCommand>
    {
        public CreateInternInfoValidator()
        {
            RuleFor(m => m.UserId)
                .NotEmpty().WithMessage("User Id không được để trống!");
            RuleFor(m => m.HoTen)
                .NotEmpty().WithMessage("Họ Tên không được để trống!")
                .MaximumLength(255);
            RuleFor(m => m.NgaySinh)
                .LessThan(DateTime.UtcNow.AddDays(7)).WithMessage("Ngày sinh phải bé hơn thời điểm hiện tại.");
            RuleFor(m => m.GioiTinh)
                .NotEmpty().WithMessage("Giới Tính không được để trống!");
            RuleFor(m => m.MSSV)
                .NotEmpty().WithMessage("MSSV không được để trống!");
            RuleFor(m => m.EmailTruong)
                .NotEmpty().WithMessage("Email Trường không được để trống!")
                .EmailAddress().WithMessage("Sai định dạng Email.")
                .MaximumLength(255);
            RuleFor(m => m.EmailCaNhan)
                .NotEmpty().WithMessage("Email cá nhân không được để trống!")
                .EmailAddress().WithMessage("Sai định dạng Email.")
                .MaximumLength(255);
            RuleFor(m => m.Sdt)
                .NotEmpty().WithMessage("Số điện thoại không được để trống!")
                .MinimumLength(10).WithMessage("Số điện thoại phải là 10 chữ số.");
            RuleFor(m => m.SdtNguoiThan)
                .NotEmpty().WithMessage("Số điện thoại không được để trống!")
                .MinimumLength(10).WithMessage("Số điện thoại phải là 10 chữ số.");
            RuleFor(m => m.DiaChi)
                .NotEmpty().WithMessage("Địa chỉ không được đê trống!");
            RuleFor(m => m.GPA)
                .GreaterThan(0).WithMessage("GPA phải lớn hơn 0.")
                .LessThanOrEqualTo(10).WithMessage("GPA không thể vượt quá 10");
            RuleFor(m => m.TrinhDoTiengAnh)
                .NotEmpty().WithMessage("Trình độ tiếng anh không được để trống!");
            RuleFor(m => m.LinkFacebook)
                .NotEmpty().WithMessage("Link Facbook không được để trống!");
            RuleFor(m => m.LinkCv)
                .NotEmpty().WithMessage("Link CV không được để trống!");
            RuleFor(m => m.NganhHoc)
                .NotEmpty().WithMessage("Ngành học không được để trống!");
            RuleFor(m => m.TrangThai)
                .NotEmpty().WithMessage("Trạng thái không được để trống!");
            RuleFor(m => m.Round)
                .NotEmpty().WithMessage("Round không được để trống!");
            RuleFor(m => m.ViTriMongMuon)
                .NotEmpty().WithMessage("Vị trí mong muốn không được để trống!")
                .MaximumLength(255);
            RuleFor(m => m.StartDate)
                .LessThan(m => m.EndDate).WithMessage("Ngày bắt đầu phải ngắn hơn ngày hoàn thành!");
            RuleFor(m => m.EndDate)
                .GreaterThan(m => m.StartDate).WithMessage("Ngày hoàn thành phải dài hơn ngày bắt đầu!");
            RuleFor(m => m.IdTruong)
                .NotEmpty().WithMessage("Id Trường không được để trống!");
            
        }
    }

    public class CreateInternInfoCommand : IRequest<CreateInternInfoResponse>
    {
        public string? UserId { get; set; }
        public string HoTen { get; set; }
        public DateTime NgaySinh { get; set; }
        public bool GioiTinh { get; set; }
        public string MSSV { get; set; }
        public string EmailTruong { get; set; }
        public string EmailCaNhan { get; set; }
        public string Sdt { get; set; }
        public string SdtNguoiThan { get; set; }
        public string DiaChi { get; set; }
        public decimal GPA { get; set; }
        public string TrinhDoTiengAnh { get; set; }
        public string LinkFacebook { get; set; }
        public string LinkCv { get; set; }
        public string NganhHoc { get; set; }
        public string TrangThai { get; set; }
        public int Round { get; set; }
        public string ViTriMongMuon { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int IdTruong { get; set; }
        public int? KyThucTapId { get; set; }
        public int? DuAnId { get; set; }
    }
}
