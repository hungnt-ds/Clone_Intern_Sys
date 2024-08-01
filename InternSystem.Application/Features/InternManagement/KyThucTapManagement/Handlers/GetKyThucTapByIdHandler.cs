using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Models;
using InternSystem.Application.Features.InternManagement.KyThucTapManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.InternManagement.KyThucTapManagement.Handlers
{
    public class GetKyThucTapByIdHandler : IRequestHandler<GetKyThucTapByIdQuery, GetKyThucTapByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetKyThucTapByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetKyThucTapByIdResponse> Handle(GetKyThucTapByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var kyThucTapRepository = _unitOfWork.GetRepository<KyThucTap>();

                var result = await kyThucTapRepository
                    .GetAllQueryable()
                    .Include(ktt => ktt.TruongHoc) 
                    .FirstOrDefaultAsync(ktt => ktt.Id == request.Id && !ktt.IsDelete, cancellationToken);

                if (result == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy kỳ thực tập.");
                }

                var response = _mapper.Map<GetKyThucTapByIdResponse>(result);
                return response;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra.");
            }
        }

    }
}
