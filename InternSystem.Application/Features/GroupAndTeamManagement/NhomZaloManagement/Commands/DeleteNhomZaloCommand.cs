using FluentValidation;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Commands;
using MediatR;

namespace InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Commands
{
    public class DeleteNhomZaloCommandValidator : AbstractValidator<DeleteNhomZaloCommand>
    {
        public DeleteNhomZaloCommandValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn nhóm Zalo để xóa!");
        }
    }

    public class DeleteNhomZaloCommand : IRequest<DeleteNhomZaloResponse>
    {
        public int Id { get; set; }
    }
}
