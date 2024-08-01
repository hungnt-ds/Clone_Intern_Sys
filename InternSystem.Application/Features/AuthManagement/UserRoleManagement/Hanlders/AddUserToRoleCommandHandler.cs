using InternSystem.Application.Common.Constants;
using InternSystem.Application.Features.AuthManagement.UserRoleManagement.Commands;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Application.Features.AuthManagement.UserRoleManagement.Hanlders
{
    public class AddUserToRoleCommandHandler : IRequestHandler<AddUserToRoleCommand, bool>
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AddUserToRoleCommandHandler(UserManager<AspNetUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> Handle(AddUserToRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng");
                }

                var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
                if (!roleExists)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy vai trò");
                }

                var result = await _userManager.AddToRoleAsync(user, request.RoleName);
                return result.Succeeded;
            }
            catch
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã xảy ra lỗi");
            }
        }
    }
}
