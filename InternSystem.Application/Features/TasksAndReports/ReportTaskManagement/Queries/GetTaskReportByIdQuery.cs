using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.ReportTaskManagement.Queries
{

    public class GetTaskReportByIdValidator : AbstractValidator<GetTaskReportByIdQuery>
    {
        public GetTaskReportByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetTaskReportByIdQuery : IRequest<GetReportByIDReponse>
    {
        public int Id { get; set; }
    }
}
