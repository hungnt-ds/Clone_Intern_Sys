using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.TaskManagement.Queries
{
    public class GetTaskByIdValidator : AbstractValidator<GetTaskByIdQuery>
    {
        public GetTaskByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetTaskByIdQuery : IRequest<TaskResponse>
    {
        public int Id { get; set; }
    }
}
