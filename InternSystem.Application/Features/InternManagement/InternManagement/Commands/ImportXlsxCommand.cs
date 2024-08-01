using FluentValidation;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Commands
{

    public class ImportXlsxValidator : AbstractValidator<ImportXlsxCommand>
    {
        public ImportXlsxValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .Must(file => file.Length > 0)
                .WithMessage("File không được để trống!")
                .Must(file => Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Chỉ .xlsx files được sử dụng.");
        }
    }

    public class ImportXlsxCommand : IRequest<UploadFileResponse>
    {
        public IFormFile File { get; set; }
    }
}
