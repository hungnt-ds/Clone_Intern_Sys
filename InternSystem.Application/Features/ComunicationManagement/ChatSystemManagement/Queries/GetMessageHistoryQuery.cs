using InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.ComunicationManagement.ChatSystemManagement.Queries
{
    public class GetMessageHistoryQuery : IRequest<List<GetMessageHistoryResponse>>
    {
        public string IdSender { get; set; }
        public string IdReceiver { get; set; }
    }
}
