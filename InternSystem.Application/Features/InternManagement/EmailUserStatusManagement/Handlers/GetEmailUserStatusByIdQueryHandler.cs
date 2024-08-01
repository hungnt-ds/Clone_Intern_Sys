using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Models;
using InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.EmailUserStatusManagement.Handlers
{
    public class GetEmailUserStatusByIdQueryHandler : IRequestHandler<GetEmailUserStatusByIdQuery, GetDetailEmailUserStatusResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetEmailUserStatusByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetDetailEmailUserStatusResponse> Handle(GetEmailUserStatusByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var existStatus = await _unitOfWork.EmailUserStatusRepository.GetByIdAsync(request.Id);
                if (existStatus == null || existStatus.IsDelete == true)
                    throw new ErrorException(StatusCodes.Status204NoContent, ResponseCodeConstants.NOT_FOUND, "Không có EmailUserStatus nào được tìm thấy");

                return _mapper.Map<GetDetailEmailUserStatusResponse>(existStatus);
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
