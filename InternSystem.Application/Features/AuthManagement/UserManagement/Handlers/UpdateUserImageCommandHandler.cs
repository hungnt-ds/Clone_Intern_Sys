using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Common.Services.Interfaces;
using InternSystem.Application.Features.AuthManagement.UserManagement.Commands;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using MediatR;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Handlers
{
    public class UpdateUserImageCommandHandler : IRequestHandler<UpdateUserImageCommand, UpdateUserImageResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;

        public UpdateUserImageCommandHandler(IFileService fileService, IUnitOfWork unitOfWork)
        {
            _fileService = fileService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UpdateUserImageResponse> Handle(UpdateUserImageCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new UpdateUserImageResponse { IsSuccess = false };
            }

            try
            {
                if (!string.IsNullOrEmpty(user.ImagePath))
                {
                    var imageName = user.ImagePath.Split('/').Last();

                    await _fileService.DeleteImageAsync(imageName, "images");
                }

                var imageUrl = await _fileService.UploadImageAsync(request.File, "images");
                user.ImagePath = "/images/" + imageUrl;
                await _unitOfWork.UserRepository.UpdateUserAsync(user);

                return new UpdateUserImageResponse
                {
                    IsSuccess = true,
                    ImageUrl = user.ImagePath
                };
            }
            catch (Exception)
            {
                return new UpdateUserImageResponse { IsSuccess = false };
            }
        }
    }
}
