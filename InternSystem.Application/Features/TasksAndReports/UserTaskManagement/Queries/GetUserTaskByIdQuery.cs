using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Queries
{
    public class GetUserTaskByIdValidator : AbstractValidator<GetUserTaskByIdQuery>
    {
        public GetUserTaskByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetUserTaskByIdQuery : IRequest<UserTaskReponse>
    {
        public int Id { get; set; }
    }
}
