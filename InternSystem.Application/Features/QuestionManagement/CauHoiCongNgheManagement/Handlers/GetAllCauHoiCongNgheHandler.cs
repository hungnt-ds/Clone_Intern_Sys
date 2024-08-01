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
    public class GetAllCauHoiCongNgheHandler : IRequestHandler<GetAllCauHoiCongNgheQuery, IEnumerable<GetAllCauHoiCongNgheResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllCauHoiCongNgheHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetAllCauHoiCongNgheResponse>> Handle(GetAllCauHoiCongNgheQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cauHoiCongNgheRepository = _unitOfWork.GetRepository<CauHoiCongNghe>();

                var listCauHoiCongNghe = await cauHoiCongNgheRepository
                    .GetAllQueryable()
                    .Include(chcn => chcn.CauHoi)                      
                    .Include(chcn => chcn.CongNghe)
                     .Where(da => da.IsActive && !da.IsDelete)
                    .ToListAsync(cancellationToken);

                if (listCauHoiCongNghe == null || !listCauHoiCongNghe.Any())
                {
                    throw new ErrorException(
                        StatusCodes.Status204NoContent,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không có câu hỏi công nghệ."
                    );
                }

                var response = _mapper.Map<IEnumerable<GetAllCauHoiCongNgheResponse>>(listCauHoiCongNghe);

                foreach (var cauHoiCongNgheResponse in response)
                {
                    cauHoiCongNgheResponse.CreatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(cauHoiCongNgheResponse.CreatedBy) ?? "Người dùng không xác định";
                    cauHoiCongNgheResponse.LastUpdatedByName = await _unitOfWork.UserRepository.GetUserNameByIdAsync(cauHoiCongNgheResponse.LastUpdatedBy) ?? "Người dùng không xác định";
      
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

        /*public async Task<IEnumerable<GetAllCauHoiCongNgheResponse>> Handle(GetAllCauHoiCongNgheQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cauHoiCongNghe = await _unitOfWork.CauHoiCongNgheRepository.GetAllAsync() ??
                               throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy câu hỏi công nghệ.");

                return _mapper.Map<IEnumerable<GetAllCauHoiCongNgheResponse>>(cauHoiCongNghe);
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
