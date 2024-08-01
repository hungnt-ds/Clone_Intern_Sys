using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Models;
using MediatR;


namespace InternSystem.Application.Features.TasksAndReports.TaskManagement.Queries
{
    public class GetTaskByMotaQueryValidator : AbstractValidator<GetTaskByMoTaQuery>
    {
        public GetTaskByMotaQueryValidator()
        {
            RuleFor(model => model.MoTa).NotEmpty().WithMessage("Mota is required");
        }
    }

    public class GetTaskByMoTaQuery : IRequest<IEnumerable<TaskResponse>>
    {
        public string MoTa { get; set; } = string.Empty;
    }
}
