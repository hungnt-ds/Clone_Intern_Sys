using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Queries
{

    public class GetDuAnByTenValidator : AbstractValidator<GetDuAnByTenQuery>
    {
        public GetDuAnByTenValidator()
        {
            RuleFor(m => m.Ten).NotNull();
        }
    }


    public class GetDuAnByTenQuery : IRequest<IEnumerable<GetDuAnByTenResponse>>
    {
        public string Ten { get; set; }
    }
}
