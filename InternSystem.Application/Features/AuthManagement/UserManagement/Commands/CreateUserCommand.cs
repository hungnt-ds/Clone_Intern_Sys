using FluentValidation;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Commands
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(model => model.HoVaTen)
                .NotEmpty().WithMessage("Họ và Tên không được bỏ trống!")
                .MinimumLength(2).WithMessage("Họ và Tên phải có ít nhất 2 ký tự.");

            RuleFor(model => model.Email)
                .NotEmpty().WithMessage("Email không được bỏ trống!")
                .EmailAddress().WithMessage("Sai định dạng Email.");

            RuleFor(model => model.PhoneNumber)
                .NotEmpty().WithMessage("Số điện thoại không được bỏ trống!")
                .Length(10).WithMessage("Số điện thoạt cần có 10 chữ số.");

            RuleFor(model => model.Username)
                .NotEmpty().WithMessage("Username không được bỏ trống!")
                .MinimumLength(6).WithMessage("Username phải có ít nhất 6 ký tự.");

            RuleFor(model => model.Password)
                .NotEmpty().WithMessage("Mật khẩu không được bỏ trống!")
                .MinimumLength(8).WithMessage("Mật khẩu phải có ít nhất 8 ký tự.")
                .MaximumLength(20).WithMessage("Mật khẩu không được vượt quá 20 ký tự.")
                .Matches("[A-Z]").WithMessage("Mật khẩu phải chứa ít nhất 1 ký tự in hoa.")
                .Matches("[a-z]").WithMessage("Mật khẩu phải chứa ít nhất 1 ký tự thường.")
                .Matches("[0-9]").WithMessage("Mật khẩu phải chứa ít nhất 1 ký tự số.")
                .Matches("[!@#$%^&*]").WithMessage("Mật khẩu phải chứa ít nhất 1 ký tự đặc biệt.");

            RuleFor(model => model.InternInfoId)
                .GreaterThan(0).WithMessage("Intern Info ID phải lớn hơn 0.");

            RuleFor(model => model.RoleName)
                .NotEmpty().WithMessage("Tên Role không được bỏ trống!");
        }
    }
    public class CreateUserCommand : IRequest<CreateUserResponse>
    {
        public required string HoVaTen { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string PhoneNumber { get; set; }
        public int? InternInfoId { get; set; }
        public required string RoleName { get; set; }
    }
}
