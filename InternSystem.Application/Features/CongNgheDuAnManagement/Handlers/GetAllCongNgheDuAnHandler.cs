using AutoMapper;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.CongNgheDuAnManagement.Models;
using InternSystem.Application.Features.CongNgheDuAnManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.CongNgheDuAnManagement.Handlers
{
    public class GetAllCongNgheDuAnHandler : IRequestHandler<GetAllCongNgheDuAnQuery, IEnumerable<GetAllCongNgheDuAnResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllCongNgheDuAnHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetAllCongNgheDuAnResponse>> Handle(GetAllCongNgheDuAnQuery request, CancellationToken cancellationToken)
        {
            var congNgheDuAns = await _unitOfWork.CongNgheDuAnRepository.GetAllAsync();
            if (congNgheDuAns == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Không tìm thấy công nghệ dự án.");
            }
            return _mapper.Map<IEnumerable<GetAllCongNgheDuAnResponse>>(congNgheDuAns);
        }
    }
}
