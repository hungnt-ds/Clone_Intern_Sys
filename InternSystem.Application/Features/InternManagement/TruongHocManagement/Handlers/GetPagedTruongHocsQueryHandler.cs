using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Models;
using InternSystem.Application.Features.InternManagement.TruongHocManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.InternManagement.TruongHocManagement.Handlers
{
    public class GetPagedTruongHocsQueryHandler : IRequestHandler<GetPagedTruongHocsQuery, PaginatedList<GetPagedTruongHocsResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedTruongHocsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedTruongHocsResponse>> Handle(GetPagedTruongHocsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var repository = _unitOfWork.GetRepository<TruongHoc>();
                var userRepository = _unitOfWork.UserRepository;

                var queryableItems = repository.GetAllQueryable()
                    .Where(th => !th.IsDelete)
                    .OrderBy(th => th.Ten);

                var paginatedItems = await PaginatedList<TruongHoc>.CreateAsync(
                    queryableItems,
                    request.PageNumber,
                    request.PageSize
                );

                //var responseItems = await Task.WhenAll(paginatedItems.Items.Select(async item =>
                //{
                //    var response = _mapper.Map<GetPagedTruongHocsResponse>(item);
                //    foreach (var duAnResponse in response)
                //    {
                //        duAnResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(duAnResponse.CreatedBy) ?? "Người dùng không xác định";
                //        duAnResponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(duAnResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                //    }

                //    response.CreatedBy = await userRepository.GetUserNameByIdAsync(item.CreatedBy) ?? "Người dùng không xác định";
                //    response.LastUpdatedBy = await userRepository.GetUserNameByIdAsync(item.LastUpdatedBy) ?? "Người dùng không xác định";

                //}).ToList());

                //var responsePaginatedList = new PaginatedList<GetPagedTruongHocsResponse>(
                //    responseItems,
                //    paginatedItems.TotalCount,
                //    paginatedItems.PageNumber,
                //    paginatedItems.PageSize
                //);

                //return responsePaginatedList;
                var responseItems = paginatedItems.Items.Select(item => _mapper.Map<GetPagedTruongHocsResponse>(item)).ToList();

                foreach (var duAnResponse in responseItems)
                {
                    duAnResponse.CreatedName = await userRepository.GetUserNameByIdAsync(duAnResponse.CreatedBy) ?? "Người dùng không xác định";
                    duAnResponse.LastUpdatedName = await userRepository.GetUserNameByIdAsync(duAnResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                }

                var responsePaginatedList = new PaginatedList<GetPagedTruongHocsResponse>(
                    responseItems,
                    paginatedItems.TotalCount,
                    paginatedItems.PageNumber,
                    paginatedItems.PageSize
                );

                return responsePaginatedList;
            }
            catch (ErrorException)
            {
                throw;
            }
            catch (Exception)
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
