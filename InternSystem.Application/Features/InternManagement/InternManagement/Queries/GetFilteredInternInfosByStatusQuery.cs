using FluentValidation;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using InternSystem.Application.Features.InternManagement.LichPhongVanManagement.Queries;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Queries
{
    public class GetFilteredInternInfosByStatusQueryValidator : AbstractValidator<GetFilteredInternInfosByStatusQuery>
    {
        public GetFilteredInternInfosByStatusQueryValidator()
        {
            RuleFor(m => m.TrangThai).NotEmpty()
                .WithMessage("Trạng thái không được bỏ trống.");
        }
    }
    public class GetFilteredInternInfosByStatusQuery : IRequest<IEnumerable<GetInternInfoResponse>>
    {
        public GetFilteredInternInfosByStatusQuery()
        {
            
        }

        public string? TrangThai { get; set; }

        public GetFilteredInternInfosByStatusQuery(string trangThai)
        {
            TrangThai = trangThai;
        }
    }
}
