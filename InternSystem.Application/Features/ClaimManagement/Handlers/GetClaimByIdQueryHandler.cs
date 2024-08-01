﻿using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.ClaimManagement.Models;
using InternSystem.Application.Features.ClaimManagement.Queries;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace InternSystem.Application.Features.ClaimManagement.Handlers
{
    public class GetClaimByIdQueryHandler : IRequestHandler<GetClaimByIdQuery, GetClaimResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetClaimByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GetClaimResponse> Handle(GetClaimByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var claim = await _unitOfWork.ClaimRepository.GetByIdAsync(request.Id);
                return _mapper.Map<GetClaimResponse>(claim);
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