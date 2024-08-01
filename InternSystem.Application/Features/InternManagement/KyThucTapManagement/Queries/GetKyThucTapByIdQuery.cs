using FluentValidation;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.KyThucTapManagement.Queries
{

    public class GetKyThucTapByIdValidator : AbstractValidator<GetKyThucTapByIdQuery>
    {
        public GetKyThucTapByIdValidator()
        {
            RuleFor(m => m.Id).NotEmpty();
        }
    }

    public class GetKyThucTapByIdQuery : IRequest<GetKyThucTapByIdResponse>
    {
        public int Id { get; set; }
    }
}
