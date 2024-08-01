using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheManagement.Queries
{
    public class GetCongNgheByTenValidator : AbstractValidator<GetCongNghesByTenQuery>
    {
        public GetCongNgheByTenValidator()
        {
            RuleFor(m => m.Ten).NotNull();
        }
    }

    public class GetCongNghesByTenQuery : IRequest<IEnumerable<GetCongNgheByTenResponse>>
    {
        public string Ten { get; set; }
    }
}
