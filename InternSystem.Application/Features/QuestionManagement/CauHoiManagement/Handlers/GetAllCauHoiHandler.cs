using AutoMapper;
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
    public class GetAllCauHoiHandler : IRequestHandler<GetAllCauHoiQuery, IEnumerable<GetAllCauHoiResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllCauHoiHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllCauHoiResponse>> Handle(GetAllCauHoiQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var cauHoiRepository = _unitOfWork.GetRepository<CauHoi>();
                var userRepository = _unitOfWork.UserRepository;

                var listCauHoi = await cauHoiRepository
                    .GetAllQueryable()
                    .Include(ch => ch.CauHoiCongNghes)
                    .ThenInclude(chcn => chcn.CongNghe)
                    .Where(ch => ch.IsActive && !ch.IsDelete)
                    .ToListAsync(cancellationToken);

                if (listCauHoi == null || !listCauHoi.Any())
                {
                    throw new ErrorException(
                        StatusCodes.Status204NoContent,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không tìm thấy câu hỏi."
                        );
                }

                var response = _mapper.Map<IEnumerable<GetAllCauHoiResponse>>(listCauHoi);

                foreach( var cauHoiResponse in response )
                {
                    cauHoiResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(cauHoiResponse.CreatedBy) ?? "Người dùng không xác định";
                    cauHoiResponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(cauHoiResponse.LastUpdatedBy) ?? "Người dùng không xác định";
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
