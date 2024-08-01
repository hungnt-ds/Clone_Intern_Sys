using InternSystem.Application.Features.InternManagement.EmailToIntern.Models;
using MediatR;

namespace InternSystem.Application.Features.InternManagement.EmailToIntern.Queries
{
    public class GetEmailsWithIndicesQuery : IRequest<IEnumerable<EmailWithIndexResponse>>
    {
    }
}
