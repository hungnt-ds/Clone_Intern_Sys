using FluentValidation;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Queries
{

    public class GetThongBaoByIdValidator : AbstractValidator<GetThongBaoByIdQuery>
    {
        public GetThongBaoByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetThongBaoByIdQuery : IRequest<GetThongBaoByIdResponse>
    {
        public int Id { get; set; }
    }
}
