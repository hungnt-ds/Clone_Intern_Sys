using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using InternSystem.Application.Features.InternManagement.InternManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class GetFilteredInternInfoByDayHandler : IRequestHandler<GetFilteredInternInfoByDayQuery, IEnumerable<GetFilteredInternInfoByDayResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetFilteredInternInfoByDayHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetFilteredInternInfoByDayResponse>> Handle(GetFilteredInternInfoByDayQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var internRepository = _unitOfWork.GetRepository<InternInfo>();
            var truongHocRepository = _unitOfWork.GetRepository<TruongHoc>();
            var kyThucTapRepository = _unitOfWork.GetRepository<KyThucTap>();
            var duAnRepository = _unitOfWork.GetRepository<DuAn>();
            var userRepository = _unitOfWork.UserRepository;

            var listIntern = await internRepository
                .GetAllQueryable()
                .Include(i => i.TruongHoc)
                .Include(i => i.KyThucTap)
                .Include(i => i.DuAn)
                .Where(da => da.IsActive && !da.IsDelete)
                .ToListAsync(cancellationToken);

            if (listIntern == null || !listIntern.Any())
            {
                throw new ErrorException(
                    StatusCodes.Status204NoContent,
                    ResponseCodeConstants.NOT_FOUND,
                    "Không tìm thấy thông tin thực tập sinh theo ngày được chọn."
                );
            }

            var response = _mapper.Map<IEnumerable<GetFilteredInternInfoByDayResponse>>(listIntern);

            foreach (var internResponse in response)
            {
                internResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(internResponse.CreatedBy) ?? "Người dùng không xác định";
                internResponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(internResponse.LastUpdatedBy) ?? "Người dùng không xác định";
            }

            return response;
        }
        catch (ErrorException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã xảy ra lỗi không mong muốn khi lưu.");
        }
    }
}