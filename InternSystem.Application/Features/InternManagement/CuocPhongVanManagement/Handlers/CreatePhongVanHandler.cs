using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Commands;
using InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace InternSystem.Application.Features.InternManagement.CuocPhongVanManagement.Handlers
{
    public class CreatePhongVanHandler : IRequestHandler<CreatePhongVanCommand, CreatePhongVanResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUserContextService _userContextService;

        public CreatePhongVanHandler(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration, IUserContextService userContextService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _userContextService = userContextService;
        }

        public async Task<CreatePhongVanResponse> Handle(CreatePhongVanCommand request, CancellationToken cancellationToken)
        {
            try
            {
                AspNetUser nguoiphongvan = await _unitOfWork.UserRepository.GetByIdAsync(request.NguoiCham);
                if (nguoiphongvan == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người chấm.");

                CauHoiCongNghe cauHoiCongNghe = await _unitOfWork.CauHoiCongNgheRepository.GetByIdAsync(request.IdCauHoiCongNghe);
                if (cauHoiCongNghe == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy câu hỏi công nghệ.");

                LichPhongVan lichPhongVan = await _unitOfWork.LichPhongVanRepository.GetByIdAsync(request.IdLichPhongVan);
                if (lichPhongVan == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy lịch phỏng vấn.");

                var passingRank = _configuration.GetValue<decimal>("PhongVanManagement:PassingRank");
                var notPass = _configuration.GetValue<string>("PhongVanManagement:NotPass");

                // Map request to PhongVan entity

                var currentUserId = _userContextService.GetCurrentUserId();
                PhongVan newPhongVan = _mapper.Map<PhongVan>(request);
                newPhongVan.LastUpdatedTime = newPhongVan.CreatedTime;
                newPhongVan.LastUpdatedBy = currentUserId;
                newPhongVan.CreatedBy = currentUserId;

                if (newPhongVan.Rank <= passingRank)
                {
                    var lichPhongVanExisting = await _unitOfWork.LichPhongVanRepository.GetByIdAsync(newPhongVan.IdLichPhongVan);
                    if (lichPhongVanExisting != null)
                    {
                        lichPhongVanExisting.KetQua = notPass;
                        await _unitOfWork.LichPhongVanRepository.UpdateAsync(lichPhongVanExisting);
                    }
                }

                // Add the new PhongVan to the repository and save changes
                newPhongVan = await _unitOfWork.PhongVanRepository.AddAsync(newPhongVan);
                await _unitOfWork.SaveChangeAsync();

                // Map the newly created PhongVan entity to CreatePhongVanResponse and return
                return _mapper.Map<CreatePhongVanResponse>(newPhongVan);

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
