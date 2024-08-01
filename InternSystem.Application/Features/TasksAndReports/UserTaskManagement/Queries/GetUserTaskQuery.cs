using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Queries
{
    public class GetUserTaskQuery : IRequest<IEnumerable<UserTaskReponse>>
    {
    }
}
