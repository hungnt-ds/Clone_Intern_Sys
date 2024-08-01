using FluentValidation;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Queries
{
    public class GetVitriByTenValidator : AbstractValidator<GetViTriByTenQuery>
    {
        public GetVitriByTenValidator()
        {
            RuleFor(m => m.Ten).NotNull();
        }
    }


    public class GetViTriByTenQuery : IRequest<IEnumerable<GetViTriByTenResponse>>
    {
        public string Ten { get; set; }
    }
}
