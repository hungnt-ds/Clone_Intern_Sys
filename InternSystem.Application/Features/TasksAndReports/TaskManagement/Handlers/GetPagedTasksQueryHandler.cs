using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Models;
using InternSystem.Application.Features.TasksAndReports.TaskManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.TasksAndReports.TaskManagement.Handlers
{
    public class GetPagedTasksQueryHandler : IRequestHandler<GetPagedTasksQuery, PaginatedList<GetPagedTasksResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedTasksQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedTasksResponse>> Handle(GetPagedTasksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var taskRepository = _unitOfWork.GetRepository<Tasks>();
                var userRepository = _unitOfWork.UserRepository;

                var tasks = taskRepository.GetAllQueryable()
                    .Include(t => t.DuAn)
                    .Where(t => t.IsActive && !t.IsDelete);

                var paginatedItems = await PaginatedList<Tasks>.CreateAsync(
                    tasks,
                    request.PageNumber,
                    request.PageSize
                );

                var responseItems = paginatedItems.Items.Select(item => _mapper.Map<GetPagedTasksResponse>(item)).ToList();

                foreach (var duAnResponse in responseItems)
                {
                    duAnResponse.CreatedName = await userRepository.GetUserNameByIdAsync(duAnResponse.CreatedBy) ?? "Người dùng không xác định";
                    duAnResponse.LastUpdatedName = await userRepository.GetUserNameByIdAsync(duAnResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                }

                var responsePaginatedList = new PaginatedList<GetPagedTasksResponse>(
                    responseItems,
                    paginatedItems.TotalCount,
                    paginatedItems.PageNumber,
                    paginatedItems.PageSize
                );

                return responsePaginatedList;
            }
            catch (ErrorException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ErrorException(
                    StatusCodes.Status500InternalServerError,
                    ResponseCodeConstants.INTERNAL_SERVER_ERROR,
                    "Đã có lỗi xảy ra"
                );
            }
        }
    }
}
