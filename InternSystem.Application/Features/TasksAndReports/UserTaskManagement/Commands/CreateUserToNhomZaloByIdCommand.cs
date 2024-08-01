using FluentValidation;
using InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.TasksAndReports.UserTaskManagement.Commands
{
    //public class GetKetQuaPhongVanByIdValidator : AbstractValidator<UserNhomZaloTaskResponse>
    //{
    //    public GetKetQuaPhongVanByIdValidator()
    //    {
    //        RuleFor(m => m.InterviewId).GreaterThan(0); // ID phải lớn hơn 0
    //    }
    //}

    public class CreateUserToNhomZaloByIdCommandValidator : AbstractValidator<CreateUserToNhomZaloByIdCommand>
    {
        public CreateUserToNhomZaloByIdCommandValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Vui lòng chọn ")
                .GreaterThan(0).WithMessage(""); 
        }
    }

    public class CreateUserToNhomZaloByIdCommand : IRequest<UserNhomZaloTaskResponse>
    {
        public int Id { get; set; }
    }
}
