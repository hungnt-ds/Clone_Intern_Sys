using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.DashboardAndStatistics.GeneralManagement.Models;
using InternSystem.Application.Features.DashboardAndStatistics.GeneralManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.DashboardAndStatistics.GeneralManagement.Handlers
{
    public class GetAllDashboardHandler : IRequestHandler<GetAllDashboardQuery, GetAllDashboardResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllDashboardHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetAllDashboardResponse> Handle(GetAllDashboardQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var dashboardList = await _unitOfWork.DashboardRepository.GetAllAsync();
                while (!dashboardList.Any())
                {
                    var newDashboard = new Dashboard();
                    await _unitOfWork.DashboardRepository.AddAsync(newDashboard);
                    await _unitOfWork.SaveChangeAsync();

                    dashboardList = await _unitOfWork.DashboardRepository.GetAllAsync();
                }
                var firstDashboard = dashboardList.First();

                firstDashboard.Interviewed = await _unitOfWork.PhongVanRepository.GetAllInterviewed();
                firstDashboard.ReceivedCV = await _unitOfWork.InternInfoRepository.GetAllReceivedCV();
                firstDashboard.Passed = await _unitOfWork.PhongVanRepository.GetAllPassed();
                firstDashboard.Interning = await _unitOfWork.InternInfoRepository.GetAllInterning();
                firstDashboard.Interned = await _unitOfWork.InternInfoRepository.GetAllInterned();

                if (dashboardList.Count() == 0)
                {
                    await _unitOfWork.DashboardRepository.AddAsync(firstDashboard);
                }
                else
                {
                    await _unitOfWork.DashboardRepository.UpdateAsync(firstDashboard);
                }

                await _unitOfWork.SaveChangeAsync();

                return new GetAllDashboardResponse
                {
                    ReceivedCV = firstDashboard.ReceivedCV,
                    Interviewed = firstDashboard.Interviewed,
                    Passed = firstDashboard.Passed,
                    Interning = firstDashboard.Interning,
                    Interned = firstDashboard.Interned
                };
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
