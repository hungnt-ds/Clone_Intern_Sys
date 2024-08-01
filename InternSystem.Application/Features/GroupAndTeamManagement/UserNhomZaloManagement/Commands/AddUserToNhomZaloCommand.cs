using FluentValidation;
using InternSystem.Application.Features.AuthManagement.LoginManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Commands
{
    public class AddUserToNhomZaloCommandValidator : AbstractValidator<AddUserToNhomZaloCommand>
    {
        public AddUserToNhomZaloCommandValidator()
        {
            RuleFor(model => model.UserId)
                .NotEmpty().WithMessage("Chưa chọn User để thêm vào nhóm!");
            RuleFor(model => model.NhomZaloId)
              .NotEmpty().WithMessage("Chưa chọn nhóm Zalo để thêm User!");
            RuleFor(model => model.IsMentor)
                .NotEmpty().WithMessage("IsMentor không được để trống!");
            RuleFor(model => model.IsLeader)
                .NotEmpty().WithMessage("IsLeader không được để trống!");
        }
    }

    public class AddUserToNhomZaloCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public int NhomZaloId { get; set; }
        public bool IsMentor { get; set; }
        public bool IsLeader { get; set; }
    }
}
