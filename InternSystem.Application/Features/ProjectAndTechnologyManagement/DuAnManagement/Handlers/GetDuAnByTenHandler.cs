using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Queries;
using InternSystem.Application.Features.QuestionManagement.CauHoiManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

public class GetDuAnsByTenQueryHandler : IRequestHandler<GetDuAnByTenQuery, IEnumerable<GetDuAnByTenResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDuAnsByTenQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetDuAnByTenResponse>> Handle(GetDuAnByTenQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var repository = _unitOfWork.GetRepository<DuAn>();
            var duAnQuery = repository.GetAllQueryable();
            var userRepository = _unitOfWork.UserRepository;

            var duAnByTen = await duAnQuery
                .Where(c => c.Ten.Contains(request.Ten) && !c.IsDelete)
                .Include(c => c.Leader)
                .Include(c => c.CongNgheDuAns)
                    .ThenInclude(cnda => cnda.CongNghe)
                .ToListAsync(cancellationToken);

            if (!duAnByTen.Any())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy dự án hợp lệ.");
            }

            var result = _mapper.Map<IEnumerable<GetDuAnByTenResponse>>(duAnByTen);
            foreach (var duAnResponse in result)
            {
                duAnResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(duAnResponse.CreatedBy) ?? "Người dùng không xác định";
                duAnResponse.LastUpdatedName = await userRepository.GetUserNameByIdAsync(duAnResponse.LastUpdatedBy) ?? "Người dùng không xác định";
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
