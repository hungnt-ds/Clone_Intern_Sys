
using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.InternManagement.Models;
using InternSystem.Application.Features.InternManagement.InternManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

public class GetFilteredInternInfoQueryByStatusHandler : IRequestHandler<GetFilteredInternInfosByStatusQuery, IEnumerable<GetInternInfoResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public GetFilteredInternInfoQueryByStatusHandler(IUnitOfWork unitOfWork, IConfiguration config, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _config = config;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetInternInfoResponse>> Handle(GetFilteredInternInfosByStatusQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var validStatuses = _config.GetSection("Trangthai").GetChildren().Select(x => x.Value).ToList();
            // neu nguoi dung nhap khac trang thai tu config se bao loi
            if (!validStatuses.Contains(request.TrangThai))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, $"Invalid TrangThai: {request.TrangThai}. Valid values are: {string.Join(", ", validStatuses)}");
            }

            var data = await _unitOfWork.InternInfoRepository.GetFilteredInternInfosByStatus(request.TrangThai);
            if (data == null)
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy thông tin thực tập sinh theo trạng thái");
            return _mapper.Map<IEnumerable<GetInternInfoResponse>>(data);
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
