using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Models;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Handlers
{
    public class GetVitrisByTenQueryHandler : IRequestHandler<GetViTriByTenQuery, IEnumerable<GetViTriByTenResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetVitrisByTenQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetViTriByTenResponse>> Handle(GetViTriByTenQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //var viTris = await _unitOfWork.ViTriRepository.GetVitrisByNameAsync(request.Ten);
                //if (viTris == null || !viTris.Any())
                //    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy vị trí");

                //return _mapper.Map<IEnumerable<GetViTriByTenResponse>>(viTris);
                var repository = _unitOfWork.GetRepository<ViTri>();
                var userRepository = _unitOfWork.UserRepository;
                var viTriQuery = repository.GetAllQueryable();

                var viTriByName = await repository.ToListAsync(
                    viTriQuery.Where(c => c.Ten == request.Ten && !c.IsDelete),
                    cancellationToken
                    );
                if (!viTriByName.Any())
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy vị trí.");
                }

                var response = _mapper.Map<IEnumerable<GetViTriByTenResponse>>(viTriByName);

                foreach ( var ViTriByTenResponse in response )
                {
                    ViTriByTenResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(ViTriByTenResponse.CreatedBy) ?? "Người dùng không xác định";
                    ViTriByTenResponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(ViTriByTenResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                }

                return response;
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
