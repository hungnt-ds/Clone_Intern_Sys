using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Models;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace InternSystem.Application.Features.InternManagement.KyThucTapManagement.Handlers
{
    public class GetKyThucTapsByTenQueryHandler : IRequestHandler<GetKyThucTapsByTenQuery, IEnumerable<GetKyThucTapsByNameResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetKyThucTapsByTenQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetKyThucTapsByNameResponse>> Handle(GetKyThucTapsByTenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var kyThucTaps = await _unitOfWork.KyThucTapRepository.GetKyThucTapsByNameAsync(request.Ten);

                if (kyThucTaps.IsNullOrEmpty())
                    throw new ErrorException(StatusCodes.Status204NoContent, ResponseCodeConstants.NOT_FOUND, "Không có kỳ thực tập nào với tên này");

                return _mapper.Map<IEnumerable<GetKyThucTapsByNameResponse>>(kyThucTaps);
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
