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
    public class GetAllEmailUserStatusQueryHandler : IRequestHandler<GetAllEmailUserStatusQuery, IEnumerable<GetDetailEmailUserStatusResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllEmailUserStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetDetailEmailUserStatusResponse>> Handle(GetAllEmailUserStatusQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allStatus = await _unitOfWork.EmailUserStatusRepository.GetAllAsync();

                if (allStatus == null || !allStatus.Any())
                {
                    throw new ErrorException(StatusCodes.Status204NoContent, ResponseCodeConstants.NOT_FOUND, "Không có EmailUserStatus nào được tìm thấy");
                }

                var filteredStatus = allStatus.Where(c => c.IsActive && !c.IsDelete);

                return _mapper.Map<IEnumerable<GetDetailEmailUserStatusResponse>>(filteredStatus);
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
