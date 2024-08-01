using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Commands;
using InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.GroupAndTeamManagement.NhomZaloManagement.Handlers
{
    public class CreateNhomZaloCommandHandler : IRequestHandler<CreateNhomZaloCommand, CreateNhomZaloResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;


        public CreateNhomZaloCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userContextService = userContextService;
            _timeService = timeService;
        }
        public async Task<CreateNhomZaloResponse> Handle(CreateNhomZaloCommand request, CancellationToken cancellationToken)
        {
            try
            {                
                string currentUserId = _userContextService.GetCurrentUserId();

                var nhomZalo = new NhomZalo
                {
                    TenNhom = request.TenNhom,
                    LinkNhom = request.LinkNhom,
                    IsNhomChung = request.IsNhomChung,
                    CreatedBy = currentUserId,
                    LastUpdatedBy = currentUserId,
                    CreatedTime = _timeService.SystemTimeNow,
                    LastUpdatedTime = _timeService.SystemTimeNow,
                    IsActive = true,
                    IsDelete = false
                };

                await _unitOfWork.NhomZaloRepository.AddAsync(nhomZalo);
                await _unitOfWork.SaveChangeAsync();

                return _mapper.Map<CreateNhomZaloResponse>(nhomZalo);
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
}
