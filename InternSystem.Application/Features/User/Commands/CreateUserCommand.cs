using FluentValidation;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternSystem.Application.Features.User.Commands
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(model => model.HoVaTen)
                .NotEmpty()
                .MinimumLength(2);
            RuleFor(model => model.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(model => model.PhoneNumber)
                .NotEmpty()
                .Length(10);
            RuleFor(model => model.Username)
                .NotEmpty()
                .MinimumLength(6);
            RuleFor(model => model.Password)
            .NotEmpty()
            .MinimumLength(8) 
            .MaximumLength(20) 
            .Matches("[A-Z]").WithMessage("Mật khẩu phải chứa ít nhất một ký tự hoa") 
            .Matches("[a-z]").WithMessage("Mật khẩu phải chứa ít nhất một ký tự thường") 
            .Matches("[0-9]").WithMessage("Mật khẩu phải chứa ít nhất một số") 
            .Matches("[!@#$%^&*]").WithMessage("Mật khẩu phải chứa ít nhất một ký tự đặc biệt"); 
            RuleFor(model => model.RoleId)
                .NotEmpty();                
        }
    }
    public class CreateUserCommand : IRequest<CreateUserResponse>
    {
        public required string HoVaTen { get; set; }
        public required string Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set;}
        public required string? PhoneNumber { get; set; }
        public int? InternInfoId { get; set; }
        public required string RoleId { get; set; }
    }
}
