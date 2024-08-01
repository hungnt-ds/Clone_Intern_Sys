using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.PaggingItems;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Models;
using InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;


namespace InternSystem.Application.Features.GroupAndTeamManagement.UserNhomZaloManagement.Handlers
{
    public class GetPagedUserNhomZaloQueryHandler : IRequestHandler<GetPagedUserNhomZaloQuery, PaginatedList<GetPagedUserNhomZaloResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPagedUserNhomZaloQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<GetPagedUserNhomZaloResponse>> Handle(GetPagedUserNhomZaloQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userNhomZaloRepository = _unitOfWork.GetRepository<UserNhomZalo>();
                var userRepository = _unitOfWork.UserRepository;
                var nhomZaloRepository = _unitOfWork.GetRepository<NhomZalo>();

                var items = userNhomZaloRepository.GetAllQueryable().Where(uz => !uz.IsDelete);

                var paginatedItems = await PaginatedList<UserNhomZalo>.CreateAsync(
                    items,
                    request.PageNumber,
                    request.PageSize
                );

                //var responseItems = new List<GetPagedUserNhomZaloResponse>();

                var responseItems = paginatedItems.Items.Select(item => _mapper.Map<GetPagedUserNhomZaloResponse>(item)).ToList();

                foreach (var response in responseItems)
                {
                    response.TenNguoiDung = await userRepository.GetUserNameByIdAsync(response.UserId) ?? "Người dùng không xác định";
                    var nhomZaloChung = await nhomZaloRepository.GetByIdAsync(response.IdNhomZaloChung);
                    response.TenNhomZaloChung = nhomZaloChung.TenNhom;
                    var nhomZaloRieng = await nhomZaloRepository.GetByIdAsync(response.IdNhomZaloRieng);
                    response.TenNhomZaloRieng = nhomZaloRieng.TenNhom;

                    response.CreatedByName = await userRepository.GetUserNameByIdAsync(response.CreatedBy) ?? "Người dùng không xác định";
                    response.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(response.LastUpdatedBy) ?? "Người dùng không xác định";
                }

                var responsePaginatedList = new PaginatedList<GetPagedUserNhomZaloResponse>(
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
