using InternSystem.Application.Common.Constants;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Commands;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Handlers
{
    public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, bool>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public UpdateRoleHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<bool> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(request.RoleId);
                if (role == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy vai trò");
                }
                role.Name = request.NewRoleName.ToLower();
                var result = await _roleManager.UpdateAsync(role);
                return result.Succeeded;
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
