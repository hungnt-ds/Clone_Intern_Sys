using FluentValidation;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Commands
{
    public class UpdateUserNhomZaloCommandValidator : AbstractValidator<UpdateUserNhomZaloCommand>
    {
        public UpdateUserNhomZaloCommandValidator()
        {
            RuleFor(model => model.Id)
                .NotEmpty().WithMessage("Id không được để trống!");
            RuleFor(model => model.isMentor)
                .NotEmpty().WithMessage("IsMentor không được để trống!");
            RuleFor(model => model.IsLeader)
                .NotEmpty().WithMessage("IsLeader không được để trống!");
            RuleFor(model => model.idNhomZaloChung)
                .NotEmpty().WithMessage("Id nhóm Zalo chung không được để trống!");
            RuleFor(model => model.idNhomZaloRieng)
                .NotEmpty().WithMessage("Id nhóm Zalo riêng không được để trống!");
        }
    }

    public class UpdateUserNhomZaloCommand : IRequest<UpdateUserNhomZaloResponse>
    {
        public int Id { get; set; }
        public bool isMentor { get; set; }
        public bool IsLeader { get; set; }
        public int? idNhomZaloChung { get; set; }
        public int? idNhomZaloRieng { get; set; }
    }
}
