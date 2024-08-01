using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using InternSystem.Application.Features.InternManagement.InternManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.InternManagement.Handlers
{
    public class GetInternInfoByTruongHocNameQueryHandler : IRequestHandler<GetInternInfoByTruongHocNameQuery, IEnumerable<GetInternInfoResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetInternInfoByTruongHocNameQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetInternInfoResponse>> Handle(GetInternInfoByTruongHocNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var internInfos = await _unitOfWork.InternInfoRepository.GetInternInfoByTenTruongHocAsync(request.TruongHocName);
                if (!internInfos.Any())
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy thông tin của thực tập sinh");
                return _mapper.Map<IEnumerable<GetInternInfoResponse>>(internInfos);
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra");
            }
        }
    }
}
