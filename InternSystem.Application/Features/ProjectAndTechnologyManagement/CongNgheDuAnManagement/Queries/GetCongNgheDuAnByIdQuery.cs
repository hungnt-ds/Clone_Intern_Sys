using FluentValidation;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Queries
{
    public class GetCongNgheDuAnByIdValidator : AbstractValidator<GetCongNgheDuAnByIdQuery>
    {
        public GetCongNgheDuAnByIdValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0);
        }
    }

    public class GetCongNgheDuAnByIdQuery : IRequest<GetCongNgheDuAnByIdResponse>
    {
        public int Id { get; set; }
    }
}
