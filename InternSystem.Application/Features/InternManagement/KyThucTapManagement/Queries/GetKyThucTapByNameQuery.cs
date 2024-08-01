using FluentValidation;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.KyThucTapManagement.Queries
{
    public class GetKyThucTapByNameValidator : AbstractValidator<GetKyThucTapByNameQuery>
    {
        public GetKyThucTapByNameValidator()
        {
            RuleFor(m => m.Ten).NotEmpty();
        }
    }

    public class GetKyThucTapByNameQuery : IRequest<IEnumerable<GetKyThucTapByNameResponse>>
    {
        public string Ten { get; set; }

        public GetKyThucTapByNameQuery()
        {
        }
    }
}
