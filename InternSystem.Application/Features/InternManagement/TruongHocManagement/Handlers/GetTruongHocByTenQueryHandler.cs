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
    public class GetTruongHocByTenQueryHandler : IRequestHandler<GetTruongHocByTenQuery, IEnumerable<GetTruongHocByNameResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTruongHocByTenQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetTruongHocByNameResponse>> Handle(GetTruongHocByTenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var truongHocRepository = _unitOfWork.GetRepository<TruongHoc>();
                var userRepository = _unitOfWork.UserRepository;

                var truongHocs = await truongHocRepository.GetAllQueryable()
                    .Where(th => th.Ten.Contains(request.TenTruong) && !th.IsDelete)
                    .ToListAsync(cancellationToken);

                if (!truongHocs.Any())
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, message: "Không tìm thấy trường học");
                }

                //var responseItems = await Task.WhenAll(truongHocs.Select(async truongHoc =>
                //{
                //    var response = _mapper.Map<GetTruongHocByNameResponse>(truongHoc);

                //    response.CreatedByName = await userRepository.GetUserNameByIdAsync(truongHoc.CreatedBy) ?? "Người dùng không xác định";
                //    response.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(truongHoc.LastUpdatedBy) ?? "Người dùng không xác định";

                //    return response;
                //}).ToList());

                var response = _mapper.Map<IEnumerable<GetTruongHocByNameResponse>>(truongHocs);

                foreach (var duAnResponse in response)
                {
                    duAnResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(duAnResponse.CreatedBy) ?? "Người dùng không xác định";
                    duAnResponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(duAnResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                }

                return response;
            }
            catch (ErrorException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, ex.ToString());
            }
        }
    }
}
