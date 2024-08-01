using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Queries
{
    public class GetAllCauHoiQuery : IRequest<IEnumerable<GetAllCauHoiResponse>>
    {
    }
}
