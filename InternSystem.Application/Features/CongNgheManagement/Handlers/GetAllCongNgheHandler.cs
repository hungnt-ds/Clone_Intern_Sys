using AutoMapper;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.CongNgheManagement.Models;
using InternSystem.Application.Features.CongNgheManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.CongNgheManagement.Handlers
{
    public class GetAllCongNgheHandler : IRequestHandler<GetAllCongNgheQuery, IEnumerable<GetAllCongNgheResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllCongNgheHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetAllCongNgheResponse>> Handle(GetAllCongNgheQuery request, CancellationToken cancellationToken)
        {
            var congNghes = await _unitOfWork.CongNgheRepository.GetAllAsync();
            if (congNghes == null || !congNghes.Any())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ErrorCode.NotFound, "Không tìm thấy công nghệ.");
            }

            return _mapper.Map<IEnumerable<GetAllCongNgheResponse>>(congNghes);
        }
    }
}
