using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Commands;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

public class UpdateNhomZaloCommandHandler : IRequestHandler<UpdateNhomZaloCommandWrapper, UpdateNhomZaloResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserContextService _userContextService;
    private readonly ITimeService _timeService;


    public UpdateNhomZaloCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        IUserContextService userContextService, ITimeService timeService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _userContextService = userContextService;
        _timeService = timeService;
    }

    public async Task<UpdateNhomZaloResponse> Handle(UpdateNhomZaloCommandWrapper request, CancellationToken cancellationToken)
    {
        try
        {
            string currentUserId = _userContextService.GetCurrentUserId();

            var existingNhomZalo = await _unitOfWork.NhomZaloRepository.GetByIdAsync(request.Id);
            if (existingNhomZalo == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy nhóm Zalo");
            }

            existingNhomZalo.TenNhom = request.Command.TenNhom;
            existingNhomZalo.LinkNhom = request.Command.LinkNhom;
            existingNhomZalo.LastUpdatedBy = currentUserId;
            existingNhomZalo.LastUpdatedTime = _timeService.SystemTimeNow;
            existingNhomZalo.IsNhomChung = request.Command.IsNhomChung;

            _unitOfWork.NhomZaloRepository.UpdateNhomZaloAsync(existingNhomZalo);
            await _unitOfWork.SaveChangeAsync();

            return new UpdateNhomZaloResponse { Id = existingNhomZalo.Id };
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