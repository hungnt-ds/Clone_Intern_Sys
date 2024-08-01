using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.UserDuAnManagement.Handlers
{
    public class GetPagedUserDuAnQueryHandler : IRequestHandler<GetPagedUserDuAnQuery, PaginatedList<GetPagedUserDuAnResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedUserDuAnQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedUserDuAnResponse>> Handle(GetPagedUserDuAnQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<UserDuAn>();
                var items = repository.GetAllQueryable();

                var paginatedItems = await PaginatedList<UserDuAn>.CreateAsync(
                    items,
                    request.PageNumber,
                    request.PageSize
                );

                var responseItems = paginatedItems.Items.Select(item => _mapper.Map<GetPagedUserDuAnResponse>(item)).ToList();

                var responsePaginatedList = new PaginatedList<GetPagedUserDuAnResponse>(
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
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra");
            }
        }
    }
}
