using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Models;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.InternManagement.TruongHocManagement.Handlers
{
    public class GetTruongHocByIdHandler : IRequestHandler<GetTruongHocByIdQuery, GetTruongHocByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTruongHocByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetTruongHocByIdResponse> Handle(GetTruongHocByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var truongHocRepository = _unitOfWork.GetRepository<TruongHoc>();
                var userRepository = _unitOfWork.UserRepository;

                var truongHoc = await truongHocRepository.GetAllQueryable()
                    .Where(th => th.Id == request.Id && !th.IsDelete)
                    .FirstOrDefaultAsync(cancellationToken);

                if (truongHoc == null)
                {
                    throw new ErrorException(
                        StatusCodes.Status404NotFound,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không tìm thấy trường học"
                    );
                }

                var response = _mapper.Map<GetTruongHocByIdResponse>(truongHoc);

                response.CreatedByName = await userRepository.GetUserNameByIdAsync(truongHoc.CreatedBy) ?? "Người dùng không xác định";
                response.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(truongHoc.LastUpdatedBy) ?? "Người dùng không xác định";

                return response;
            }
            catch (ErrorException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ErrorException(
                    StatusCodes.Status500InternalServerError,
                    ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                    "Đã có lỗi xảy ra."
                );
            }
        }
    }
}
