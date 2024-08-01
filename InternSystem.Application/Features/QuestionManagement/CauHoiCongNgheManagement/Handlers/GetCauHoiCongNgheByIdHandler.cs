using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Models;
using InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiCongNgheManagement.Handlers
{
    public class GetCauHoiCongNgheByIdHandler : IRequestHandler<GetCauHoiCongNgheByIdQuery, GetCauHoiCongNgheByIdResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetCauHoiCongNgheByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetCauHoiCongNgheByIdResponse> Handle(GetCauHoiCongNgheByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cauHoiCongNgheRepository = _unitOfWork.GetRepository<CauHoiCongNghe>();

                var cauHoiCongNghe = await cauHoiCongNgheRepository
                    .GetAllQueryable()
                    .Include(chcn => chcn.CauHoi)
                    .Include(chcn => chcn.CongNghe)
                     .Where(da => da.IsActive && !da.IsDelete)
                    .FirstOrDefaultAsync(chcn => chcn.Id == request.Id, cancellationToken);

                if (cauHoiCongNghe == null || cauHoiCongNghe.IsDelete)
                {
                    throw new ErrorException(
                        StatusCodes.Status404NotFound,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không tìm thấy câu hỏi công nghệ."
                    );
                }

                var response = _mapper.Map<GetCauHoiCongNgheByIdResponse>(cauHoiCongNghe);

                response.CreatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(response.CreatedBy) ?? "Người dùng không xác định";
                response.LastUpdatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(response.LastUpdatedBy) ?? "Người dùng không xác định";

                
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

        /* public async Task<GetCauHoiCongNgheByIdResponse> Handle(GetCauHoiCongNgheByIdQuery request, CancellationToken cancellationToken)
         {            
             try
             {
                 CauHoiCongNghe? cauHoiCongNghe = await _unitOfWork.CauHoiCongNgheRepository.GetByIdAsync(request.Id)
                     ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Danh mục không tồn tại.");
                 var result = cauHoiCongNghe.IsDelete == true
                     ? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy danh mục.")
                     : _mapper.Map<GetCauHoiCongNgheByIdResponse>(cauHoiCongNghe);
                 return result;
             }
             catch (ErrorException ex)
             {
                 throw;
             }
             catch (Exception ex)
             {
                 throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra");
             }
         }*/
    }
}
