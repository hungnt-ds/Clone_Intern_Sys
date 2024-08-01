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
    public class GetAllTruongHocHandler : IRequestHandler<GetAllTruongHocQuery, IEnumerable<GetAllTruongHocResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllTruongHocHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllTruongHocResponse>> Handle(GetAllTruongHocQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var truongHocRepository = _unitOfWork.GetRepository<TruongHoc>();
                var userRepository = _unitOfWork.UserRepository;

                var activeTruongHoc = await truongHocRepository.GetAllQueryable()
                    .Where(p => !p.IsDelete)
                    .OrderByDescending(p => p.Ten)
                    .ToListAsync(cancellationToken);

                if (!activeTruongHoc.Any())
                {
                    throw new ErrorException(
                        StatusCodes.Status404NotFound,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không tìm thấy trường học"
                    );
                }

                var response = _mapper.Map<IEnumerable<GetAllTruongHocResponse>>(activeTruongHoc);

                foreach (var item in response)
                {
                    item.CreatedName = await userRepository.GetUserNameByIdAsync(item.CreatedBy);
                    item.LastUpdatedName = await userRepository.GetUserNameByIdAsync(item.LastUpdatedBy);
                }

                return response;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(
                    StatusCodes.Status500InternalServerError,
                    ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                    "Đã có lỗi xảy ra"
                );
            }
        }
    }
}
