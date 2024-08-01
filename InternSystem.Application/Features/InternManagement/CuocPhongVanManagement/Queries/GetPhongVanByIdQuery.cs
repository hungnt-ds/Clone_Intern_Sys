using FluentValidation;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Queries
{
    public class GetPhongVanByIdValidator : AbstractValidator<GetPhongVanByIdQuery>
    {
        public GetPhongVanByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0); //
        }
    }

    public class GetPhongVanByIdQuery : IRequest<GetPhongVanByIdResponse>
    {
        public int Id { get; set; }
    }
}

