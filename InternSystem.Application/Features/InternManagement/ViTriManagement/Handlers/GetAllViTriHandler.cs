using System.Diagnostics;
using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Models;
using InternSystem.Application.Features.InternManagement.ViTriManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.InternManagement.ViTriManagement.Handlers
{
    public class GetAllViTriHandler : IRequestHandler<GetAllViTriQuery, IEnumerable<GetAllViTriResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetAllViTriHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetAllViTriResponse>> Handle(GetAllViTriQuery request, CancellationToken cancellationToken)
        {
            try
            {
                //var stopwatch = new Stopwatch();

                //// Phương thức 1
                //stopwatch.Start();
                //IQueryable<ViTri> allViTri = _unitOfWork.ViTriRepository.Entities;
                //IQueryable<ViTri> activeViTri = allViTri.Where(p => !p.IsDelete).OrderByDescending(p => p.CreatedTime);
                //var allViTriList = await activeViTri.ToListAsync()
                //    ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy lịch phỏng vấn");
                //stopwatch.Stop();
                //var time1 = stopwatch.ElapsedMilliseconds;
                //stopwatch.Reset();
                ////return _mapper.Map<IEnumerable<GetAllViTriResponse>>(allViTriList);

                //// Phương thức 1
                //stopwatch.Start();
                //var ListViTri = await _unitOfWork.GetRepository<ViTri>().GetAllAsync();
                //var newL = ListViTri.Where(p => !p.IsDelete).OrderByDescending(p => p.CreatedTime);
                //stopwatch.Stop();
                //var time2 = stopwatch.ElapsedMilliseconds;
                //stopwatch.Reset();
                ////return _mapper.Map<IEnumerable<GetAllViTriResponse>>(ListViTri);

                // Phương thức 3
                //stopwatch.Start();
                // var allViTri_1 = await _unitOfWork.ViTriRepository.GetAllIQueryableAsync();
                // var activeViTri_1 = allViTri_1
                //     .Where(p => !p.IsDelete)
                //     .OrderByDescending(p => p.CreatedTime);

                // var allViTriList1 = await activeViTri_1.ToListAsync()
                //     ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy vị trí");
                ////stopwatch.Stop();
                ////var time3 = stopwatch.ElapsedMilliseconds;

                // var filteredViTriList = await activeViTri_1
                // .Where(v => v.IsActive == true && v.IsDelete == false)
                // .ToListAsync()
                // ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy vị trí");

                // //await Console.Out.WriteLineAsync($"Method 1 Average Time: {time1} milliseconds");
                // //await Console.Out.WriteLineAsync($"Method 2 Average Time: {time2} milliseconds");
                // //await Console.Out.WriteLineAsync($"Method 3 Average Time: {time3} milliseconds");
                // return _mapper.Map<IEnumerable<GetAllViTriResponse>>(filteredViTriList);
                var viTriRepositopry = _unitOfWork.GetRepository<ViTri>();
                var userRepository = _unitOfWork.UserRepository;

                var listViTri = await viTriRepositopry
                    .GetAllQueryable()
                    .Where(vt => vt.IsActive && !vt.IsDelete)
                    .ToListAsync(cancellationToken);

                if(listViTri ==null ||! listViTri.Any())
                {
                    throw new ErrorException(StatusCodes.Status204NoContent, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Vị Trí.");
                }
                
                var response =_mapper.Map<IEnumerable<GetAllViTriResponse>>(listViTri);

                foreach(var viTriResponse in response)
                {
                    viTriResponse.CreatedByName = await userRepository.GetUserNameByIdAsync(viTriResponse.CreatedBy) ?? "Người dùng không xác định";
                    viTriResponse.LastUpdatedByName = await userRepository.GetUserNameByIdAsync(viTriResponse.LastUpdatedBy) ?? "Người dùng không xác định";
                }

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
