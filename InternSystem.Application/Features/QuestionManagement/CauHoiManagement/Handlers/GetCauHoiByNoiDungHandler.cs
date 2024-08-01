using AutoMapper;
using Azure;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Models;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Handlers
{
    public class GetCauHoiByNoiDungHandler : IRequestHandler<GetCauHoiByNoiDungQuery, IEnumerable<GetCauHoiByNoiDungResponse>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCauHoiByNoiDungHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<GetCauHoiByNoiDungResponse>> Handle(GetCauHoiByNoiDungQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //var cauHoiByNoiDung = (await _unitOfWork.CauHoiRepository.GetCauHoiByNoiDungAsync(request.noidung)
                //    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Câu hỏi không tồn tại."));
                //var result = _mapper.Map<IEnumerable<GetCauHoiByNoiDungResponse>>(cauHoiByNoiDung);
                //return result;

                //var cauHoiQuery = _unitOfWork.GetRepository<CauHoi>().GetAllQueryable();

                //var cauHoiByNoiDung = cauHoiQuery
                //    .Where(c => c.NoiDung.Contains(request.noidung) && !c.IsDelete)
                //    .ToList();

                   //if (!cauHoiByNoiDung.Any())
                //{
                //    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy câu hỏi hợp lệ.");
                //}

                //var result = _mapper.Map<IEnumerable<GetCauHoiByNoiDungResponse>>(cauHoiByNoiDung);
                //return result;

                var repository = _unitOfWork.GetRepository<CauHoi>();
                var userRepository = _unitOfWork.UserRepository;
                var cauHoiQuery = repository.GetAllQueryable();

                var cauHoiByNoiDung = await cauHoiQuery
                    .Include(ch => ch.CauHoiCongNghes)
                        .ThenInclude(chcn => chcn.CongNghe)
                    .Where(c => c.NoiDung.Contains(request.noidung) && !c.IsDelete)
                    .ToListAsync(cancellationToken);

                if (cauHoiByNoiDung == null || !cauHoiByNoiDung.Any())
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy câu hỏi hợp lệ.");
                }

                var result = _mapper.Map<IEnumerable<GetCauHoiByNoiDungResponse>>(cauHoiByNoiDung);

                foreach (var cauHoiResponse in result)
                {
                    cauHoiResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(cauHoiResponse.CreatedBy) ?? "Người dùng không xác định";
                    cauHoiResponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(cauHoiResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                }

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
        }
    }
}
