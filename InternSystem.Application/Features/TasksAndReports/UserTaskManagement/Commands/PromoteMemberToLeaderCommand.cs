using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Commands
{
    public class PromoteMemberValidator : AbstractValidator<PromoteMemberToLeaderCommand>
    {
        public PromoteMemberValidator()
        {
            RuleFor(m => m.MemberId)
                .NotEmpty().WithMessage("MemberId không được để trống!");
            RuleFor(m => m.NhomZaloId)
                .NotEmpty().WithMessage("Id nhóm Zalo không được để trống!");
            //RuleFor(m => m.DuanId)
            //    .NotEmpty().WithMessage("Id Dự án không được để trống!");
        }
    }

    public class PromoteMemberToLeaderCommand : IRequest<ExampleResponse>
    {
        public string MemberId { get; set; }
        public int NhomZaloId { get; set; }
        public int? DuanId { get; set; }
    }
}
