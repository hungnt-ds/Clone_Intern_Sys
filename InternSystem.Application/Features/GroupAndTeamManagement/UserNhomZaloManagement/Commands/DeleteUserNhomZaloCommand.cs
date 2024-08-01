using FluentValidation;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Commands
{
    public class DeleteUserNhomZaloCommandValidator : AbstractValidator<DeleteUserNhomZaloCommand>
    {
        public DeleteUserNhomZaloCommandValidator()
        {
            RuleFor(model => model.Id)
                .NotEmpty().WithMessage("Chưa chọn User để xóa khỏi nhóm!");
        }
    }

    public class DeleteUserNhomZaloCommand : IRequest<DeleteUserNhomZaloResponse>
    {
        public int Id { get; set; }
    }

}
