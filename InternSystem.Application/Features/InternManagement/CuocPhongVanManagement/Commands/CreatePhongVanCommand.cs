using FluentValidation;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Commands
{
    public class CreatePhongVanValidator : AbstractValidator<CreatePhongVanCommand>
    {
        public CreatePhongVanValidator()
        {
            RuleFor(m => m.CauTraLoi)
                .NotEmpty().WithMessage("Câu trả lời không được để trống!");
            RuleFor(m => m.Rank)
                .NotEmpty().WithMessage("Chưa có điểm cho câu trả lời.")
                .GreaterThanOrEqualTo(0).WithMessage("Điểm phải lớn hơn hoặc bằng 0.");
            RuleFor(m => m.NguoiCham)
                .NotEmpty().WithMessage("Cần có người chấm điểm.");
            RuleFor(m => m.RankDate)
                .NotEmpty().WithMessage("Chưa có ngày chấm điểm.");
            RuleFor(m => m.IdCauHoiCongNghe)
                .NotEmpty().WithMessage("Chưa chọn câu hỏi công nghệ")
                .GreaterThan(0).WithMessage("Id câu hỏi công nghệ phải lớn hơn 0.");
            RuleFor(m => m.IdLichPhongVan)
                .NotEmpty().WithMessage("Chưa chọn lịch phỏng vấn")
                .GreaterThan(0).WithMessage("Id lịch phỏng vấn phải lớn hơn 0.");
        }
    }
    public class CreatePhongVanCommand : IRequest<CreatePhongVanResponse>
    {
        public string CauTraLoi { get; set; }
        public decimal Rank { get; set; }
        public string NguoiCham { get; set; }
        public DateTime RankDate { get; set; }
        public int IdCauHoiCongNghe { get; set; }
        public int IdLichPhongVan { get; set; }
    }
}
