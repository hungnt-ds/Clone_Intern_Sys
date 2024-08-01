using FluentValidation;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Queries
{
    public class GetLichPhongVanByIdValidator : AbstractValidator<GetLichPhongVanByIdQuery>
    {
        public GetLichPhongVanByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetLichPhongVanByIdQuery : IRequest<GetLichPhongVanByIdResponse>
    {
        public int Id { get; set; }
    }
}

