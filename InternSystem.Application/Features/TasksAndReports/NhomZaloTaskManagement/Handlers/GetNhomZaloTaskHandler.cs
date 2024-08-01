using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Models;
using InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.TasksAndReports.NhomZaloTaskManagement.Handlers
{
    internal class GetNhomZaloTaskHandler : IRequestHandler<GetNhomZaloTaskByQuery, IEnumerable<NhomZaloTaskReponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetNhomZaloTaskHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<NhomZaloTaskReponse>> Handle(GetNhomZaloTaskByQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var listNhomZaloTask = await _unitOfWork.NhomZaloTaskRepository.GetAllAsync();
                if (!listNhomZaloTask.Any())
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy nhóm Zalo này đang thực hiện task");
                }
                return _mapper.Map<IEnumerable<NhomZaloTaskReponse>>(listNhomZaloTask);
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
