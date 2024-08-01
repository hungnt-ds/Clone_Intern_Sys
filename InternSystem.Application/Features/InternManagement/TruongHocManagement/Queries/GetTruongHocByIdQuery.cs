using FluentValidation;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.TruongHocManagement.Queries
{
    public class GetTruongHocByIdValidator : AbstractValidator<GetTruongHocByIdQuery>
    {
        public GetTruongHocByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetTruongHocByIdQuery : IRequest<GetTruongHocByIdResponse>
    {
        public int Id { get; set; }
    }
}
