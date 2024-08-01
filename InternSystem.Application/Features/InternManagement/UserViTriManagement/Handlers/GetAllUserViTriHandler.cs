using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Models;
using InternSystem.Application.Features.InternManagement.UserViTriManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.InternManagement.UserViTriManagement.Handlers
{
    public class GetAllUserViTriHandler : IRequestHandler<GetAllUserViTriQuery, IEnumerable<GetAllUserViTriResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllUserViTriHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllUserViTriResponse>> Handle(GetAllUserViTriQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var ListUserViTri = await _unitOfWork.UserViTriRepository.GetAllQueryable()
                    .Include(uv => uv.AspNetUser)
                    .Include(uv => uv.ViTri)
                    .Where(uv => uv.IsActive && !uv.IsDelete)
                    .ToListAsync(cancellationToken);

                var response = _mapper.Map<IEnumerable<GetAllUserViTriResponse>>(ListUserViTri);

                foreach (var userViTriResponse in response)
                {
                    userViTriResponse.CreatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(userViTriResponse.CreatedBy!) ?? "Người dùng không xác định";
                    userViTriResponse.LastUpdatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(userViTriResponse.LastUpdatedBy!) ?? "Người dùng không xác định";
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
