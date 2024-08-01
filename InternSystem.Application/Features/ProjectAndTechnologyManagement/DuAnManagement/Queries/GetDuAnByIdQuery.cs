using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Queries
{

    public class GetDuAnByIdValidator : AbstractValidator<GetDuAnByIdQuery>
    {
        public GetDuAnByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetDuAnByIdQuery : IRequest<GetDuAnByIdResponse>
    {
        public int Id { get; set; }
    }
}
