using System.Text.Json.Serialization;
using FluentValidation;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Commands
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(model => model.Id)
                .NotEmpty().WithMessage("Chưa chọn User để Update!"); ;

            RuleFor(model => model.HoVaTen)
                .MinimumLength(2).WithMessage("Họ và Tên phải chứa ít nhất 2 ký tự.");

            RuleFor(model => model.Email)
                .EmailAddress().WithMessage("Sai định dạng Email.");

            RuleFor(model => model.PhoneNumber)
                .Length(10).WithMessage("Số điện thoạt cần có 10 chữ số.");

            RuleFor(model => model.InternInfoId)
                .GreaterThan(0).WithMessage("Intern Info ID phải lớn hơn 0.");

            RuleFor(model => model.RoleName)
                .NotEmpty().WithMessage("Tên Role không được bỏ trống!");
        }
    }
    public class UpdateUserCommand : IRequest<CreateUserResponse>
    {
        public required string Id { get; set; }
        public string? HoVaTen { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int? InternInfoId { get; set; }
        [JsonIgnore]
        public string? RoleName { get; set; }
    }
}
