using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Models;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Handlers
{
    public class GetPagedViTrisQueryHandler : IRequestHandler<GetPagedViTrisQuery, PaginatedList<GetPagedViTrisResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedViTrisQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedViTrisResponse>> Handle(GetPagedViTrisQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<ViTri>();
                var items = repository.GetAllQueryable();

                var paginatedItems = await PaginatedList<ViTri>.CreateAsync(
                    items,
                    request.PageNumber,
                    request.PageSize
                );

                var responseItems = paginatedItems.Items.Select(item => _mapper.Map<GetPagedViTrisResponse>(item)).ToList();

                var responsePaginatedList = new PaginatedList<GetPagedViTrisResponse>(
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
