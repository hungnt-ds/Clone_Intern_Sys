using FluentValidation;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.EmailToIntern.Commands
{
    public class SelectEmailsCommandValidator : AbstractValidator<SelectEmailsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SelectEmailsCommandValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(model => model.Indices)
                .NotEmpty().WithMessage("Indices không được để trống!")
                .MustAsync(BeValidIndices).WithMessage("Indices  phải là số nguyên không âm và nhỏ hơn độ dài của email có sẵn.");
        }

        private async Task<bool> BeValidIndices(List<int> indices, CancellationToken cancellationToken)
        {
            int count = (await _unitOfWork.InternInfoRepository.GetAllAsync()).Count();
            foreach (var index in indices)
            {
                if (index < 0 || index >= count)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class SelectEmailsCommand : IRequest<IEnumerable<string>>
    {
        public List<int> Indices { get; set; }

        public SelectEmailsCommand(List<int> indices)
        {
            Indices = indices;
        }
    }
}
