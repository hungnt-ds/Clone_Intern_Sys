using System.Text.Json.Serialization;
using FluentValidation;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Commands
{
    public class UpdateUserViTriValidator : AbstractValidator<UpdateUserViTriCommand>
    {
        public UpdateUserViTriValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id User Vị Trí không được để trống!")
                .GreaterThan(0).WithMessage("Id User Vị Trí phải lớn hơn 0.");
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id Vị Trí không được để trống!")
                .GreaterThan(0).WithMessage("Id Vị Trí phải lớn hơn 0.");
        }
    }
    public class UpdateUserViTriCommand : IRequest<UpdateUserViTriResponse>
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public int IdViTri { get; set; }
        [JsonIgnore]
        public string? LastUpdatedBy { get; set; }
    }
}
