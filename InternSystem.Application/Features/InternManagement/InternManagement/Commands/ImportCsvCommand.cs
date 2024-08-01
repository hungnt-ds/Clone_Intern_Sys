using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Commands
{
    public class ImportUsersValidator : AbstractValidator<ImportCsvCommand>
    {
        public ImportUsersValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .Must(file => file.Length > 0)
                .WithMessage("File không được để trống!")
                .Must(file => Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Chỉ .csv files được sử dụng.");
        }
    }

    public class ImportCsvCommand : IRequest<Unit>
    {
        public IFormFile File { get; set; }
    }
}
