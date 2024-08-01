using FluentValidation;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Commands;
using MediatR;

namespace InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Commands
{
    public class CreateNhomZaloCommandValidator : AbstractValidator<CreateNhomZaloCommand>
    {
        public CreateNhomZaloCommandValidator()
        {
            RuleFor(m => m.TenNhom)
                .NotEmpty().WithMessage("Tên nhóm không được để trống!");
            RuleFor(m => m.LinkNhom)
                .NotEmpty().WithMessage("Chưa cập nhật Link nhóm.");
            RuleFor(m => m.IsNhomChung)
                .NotEmpty().WithMessage("Chưa cập nhật trạng thái nhóm chung.");
        }
    }

    public class CreateNhomZaloCommand : IRequest<CreateNhomZaloResponse>
    {
        public string TenNhom { get; set; }
        public string LinkNhom { get; set; }
        public bool IsNhomChung { get; set; }
    }
}
