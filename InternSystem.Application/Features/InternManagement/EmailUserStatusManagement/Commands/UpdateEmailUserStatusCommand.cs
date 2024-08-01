using FluentValidation;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Commands
{
    public class UpdateEmailUserStatusCommandValidator : AbstractValidator<UpdateEmailUserStatusCommand>
    {
        public UpdateEmailUserStatusCommandValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Chưa chọn EmailUserStatus để Update!")
                .GreaterThan(0).WithMessage("Id người được Comment phải lớn hơn 0.");

            RuleFor(m => m.EmailLoai1)
            .NotEmpty().WithMessage("Email loại 1 không được để trống.")
            .EmailAddress().WithMessage("Email loại 1 phải là địa chỉ Email hợp lệ.");

            RuleFor(m => m.EmailLoai2)
                .NotEmpty().WithMessage("Email loại 2 không được để trống.")
                .EmailAddress().WithMessage("Email loại 2 phải là địa chỉ Email hợp lệ.");

            RuleFor(m => m.EmailLoai3)
                .NotEmpty().WithMessage("Email loại 3 không được để trống.")
                .EmailAddress().WithMessage("Email loại 3 phải là địa chỉ Email hợp lệ.");
        }
    }
    public class UpdateEmailUserStatusCommand : IRequest<GetDetailEmailUserStatusResponse>
    {
        public int Id { get; set; }
        public string EmailLoai1 { get; set; }
        public string EmailLoai2 { get; set; }
        public string EmailLoai3 { get; set; }
    }
}
