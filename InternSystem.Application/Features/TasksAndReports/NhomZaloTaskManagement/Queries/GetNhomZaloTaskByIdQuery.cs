using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Queries
{
    public class GetNhomZaloTaskByIdValidator : AbstractValidator<NhomZaloTaskReponse>
    {
        public GetNhomZaloTaskByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetNhomZaloTaskByIdQuery : IRequest<NhomZaloTaskReponse>
    {
        public int Id { get; set; }
    }

}
