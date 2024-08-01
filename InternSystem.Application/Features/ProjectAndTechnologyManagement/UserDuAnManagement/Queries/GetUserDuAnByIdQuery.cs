using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Queries
{

    public class GetUserDuAnByIdValidator : AbstractValidator<GetUserDuAnByIdQuery>
    {
        public GetUserDuAnByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetUserDuAnByIdQuery : IRequest<GetUserDuAnByIdResponse>
    {
        public int Id { get; set; }
    }
}
