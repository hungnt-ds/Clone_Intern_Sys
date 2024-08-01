using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Commands;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Handlers
{
    public class CreateDuAnHandler : IRequestHandler<CreateDuAnCommand, CreateDuAnResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        private readonly ITimeService _timeService;
        public CreateDuAnHandler(IUnitOfWork unitOfWork, IMapper mapper, IUserContextService userContextService, ITimeService timeService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContextService = userContextService;
            _timeService = timeService;
        }

        public async Task<CreateDuAnResponse> Handle(CreateDuAnCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUserId = _userContextService.GetCurrentUserId();

                DuAn? existingDuAn = _unitOfWork.DuAnRepository
                    .GetAllAsync().Result
                    .AsQueryable()
                    .FirstOrDefault(d => d.Ten.Equals(request.Ten));
                if (existingDuAn != null)
                    throw new ErrorException(StatusCodes.Status409Conflict, ErrorCode.NotUnique, "Trùng tên Dự án");

                DuAn newDuAn = _mapper.Map<DuAn>(request);
                newDuAn.LastUpdatedBy = currentUserId;
                newDuAn.CreatedBy = currentUserId;
                newDuAn.CreatedTime = _timeService.SystemTimeNow;
                newDuAn.LastUpdatedTime = _timeService.SystemTimeNow;
                newDuAn.IsActive = true;
                newDuAn.IsDelete = false;
                newDuAn = await _unitOfWork.DuAnRepository.AddAsync(newDuAn);

                await _unitOfWork.SaveChangeAsync();
                return _mapper.Map<CreateDuAnResponse>(newDuAn);


                //Cancellation token
                //newDuAn = await _unitOfWork.DuAnRepository.AddAsync(newDuAn, cancellationToken);

                //await _unitOfWork.SaveChangeAsync(cancellationToken);

                //return _mapper.Map<CreateDuAnResponse>(newDuAn);
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
