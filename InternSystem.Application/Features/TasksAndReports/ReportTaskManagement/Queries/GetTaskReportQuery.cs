using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Queries
{
    public class GetTaskReportQuery : IRequest<IEnumerable<GetReportAllReponse>>
    {
    }
}
