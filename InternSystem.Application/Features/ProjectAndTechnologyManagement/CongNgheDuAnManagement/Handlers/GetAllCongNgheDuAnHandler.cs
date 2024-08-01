using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.ProjectAndTechnologyManagement.CongNgheDuAnManagement.Handlers
{
    public class GetAllCongNgheDuAnHandler : IRequestHandler<GetAllCongNgheDuAnQuery, IEnumerable<GetAllCongNgheDuAnResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllCongNgheDuAnHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GetAllCongNgheDuAnResponse>> Handle(GetAllCongNgheDuAnQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var congNgheDuAnRepository = _unitOfWork.GetRepository<CongNgheDuAn>();

                var listCongNgheDuAn = await congNgheDuAnRepository
                    .GetAllQueryable()
                    .Include(cnda => cnda.DuAn)
                    .Include(cnda => cnda.CongNghe)
                    .Where(cnda => cnda.IsActive && !cnda.IsDelete)
                    .ToListAsync(cancellationToken);

                if (listCongNgheDuAn == null || !listCongNgheDuAn.Any())
                {
                    throw new ErrorException(
                        StatusCodes.Status204NoContent,
                        ResponseCodeConstants.NOT_FOUND,
                        "Không có công nghệ dự án."
                    );
                }

                var response = _mapper.Map<IEnumerable<GetAllCongNgheDuAnResponse>>(listCongNgheDuAn);
                
                return response;
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
