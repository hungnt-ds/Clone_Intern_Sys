using InternSystem.Application.Common.Constants;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Commands;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Handlers
{
    public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public DeleteRoleHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _roleManager.FindByNameAsync(request.Name.ToLower());
                if (role == null)
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Vai trò không tồn tại.");
                }

                var result = await _roleManager.DeleteAsync(role);
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
