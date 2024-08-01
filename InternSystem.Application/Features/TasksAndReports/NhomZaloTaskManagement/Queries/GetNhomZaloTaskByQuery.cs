using InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Queries
{
    public class GetNhomZaloTaskByQuery : IRequest<IEnumerable<NhomZaloTaskReponse>>
    {
    }
}
