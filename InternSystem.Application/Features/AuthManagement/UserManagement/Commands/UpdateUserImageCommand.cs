using FluentValidation;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Commands
{
    public class UpdateUserImageCommandValidator : AbstractValidator<UpdateUserImageCommand>
    {
        public UpdateUserImageCommandValidator()
        {
            RuleFor(model => model.UserId)
                .NotEmpty().WithMessage("Chưa chọn User để cập nhật Image!");
            RuleFor(model => model.File)
                .NotEmpty().WithMessage("File không được bỏ trống!");
        }
    }

    public class UpdateUserImageCommand : IRequest<UpdateUserImageResponse>
    {
        public string UserId { get; set; }
        public IFormFile File { get; set; }
    }    
}
