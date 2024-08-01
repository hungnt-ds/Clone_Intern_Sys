using FluentValidation;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Commands
{
    public class CreateLichPhongVanValidator : AbstractValidator<CreateLichPhongVanCommand>
    {
        public CreateLichPhongVanValidator()
        {
            RuleFor(m => m.IdNguoiPhongVan)
                .NotEmpty().WithMessage("Chưa chọn người phỏng vấn.");
            RuleFor(m => m.IdNguoiDuocPhongVan)
                .NotEmpty().WithMessage("Chưa chọn người được phỏng vấn.");
            RuleFor(m => m.ThoiGianPhongVan)
                .GreaterThan(DateTime.Now)
                .WithMessage("Thời gian phỏng vấn phải là một thời điểm trong tương lai.");
            RuleFor(m => m.DiaDiemPhongVan)
                .NotEmpty().WithMessage("Chưa lựa chọn địa điểm phỏng vấn.");
            RuleFor(m => m.DaXacNhanMail)
                .NotEmpty().WithMessage("Chưa cập nhật trạng thái xác nhận mail.");
            RuleFor(m => m.SendMailResult)
                .NotEmpty().WithMessage("Cần cập nhật kết quả send Mail.");
            RuleFor(m => m.InterviewForm)
                .NotEmpty().WithMessage("InterviewForm đang để trống");
            RuleFor(m => m.TrangThai)
                .NotEmpty().WithMessage("Chưa cập nhật trạng thái.");
            RuleFor(m => m.TimeDuration)
                .NotEmpty().WithMessage("Chưa đặt thời gian cho cuộc phỏng vấn");
            RuleFor(m => m.KetQua)
                .NotEmpty().WithMessage("Chưa cập nhật Kết quả.");
        }
    }

    public class CreateLichPhongVanCommand : IRequest<CreateLichPhongVanResponse>
    {
        public string IdNguoiPhongVan { get; set; }
        public int IdNguoiDuocPhongVan { get; set; }
        public DateTime ThoiGianPhongVan { get; set; }
        public string DiaDiemPhongVan { get; set; }
        public bool DaXacNhanMail { get; set; }
        public string? SendMailResult { get; set; }
        public string? InterviewForm { get; set; }
        public bool TrangThai { get; set; }
        public string? TimeDuration { get; set; }
        public string? KetQua { get; set; }
    }
}
