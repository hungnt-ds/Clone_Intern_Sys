using FluentValidation;
using InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ComunicationManagement.ThongBaoManagement.Queries
{
    public class GetAllThongBaoValidator : AbstractValidator<GetAllThongBaoQuery>
    {
        public GetAllThongBaoValidator()
        {

        }
    }

    public class GetAllThongBaoQuery : IRequest<IEnumerable<GetAllThongBaoResponse>>
    {
        //public int? PageNumber { get; set; }
        //public int? PageSize { get; set; }
    }
}
