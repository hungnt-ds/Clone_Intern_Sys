using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.TaskManagement.Queries
{
    public class GetTaskByNoiDungQueryValidator : AbstractValidator<GetTaskByNoiDungQuery>
    {
        public GetTaskByNoiDungQueryValidator()
        {
            RuleFor(model => model.noidung).NotEmpty().WithMessage("Noi dung is required");
        }
    }

    public class GetTaskByNoiDungQuery : IRequest<IEnumerable<TaskResponse>>
    {
        public string noidung { get; set; } = string.Empty;
    }
}
