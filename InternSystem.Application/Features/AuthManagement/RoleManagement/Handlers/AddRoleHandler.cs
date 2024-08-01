using InternSystem.Application.Common.Constants;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Commands;
using InternSystem.Domain.BaseException;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Handlers
{
    public class AddRoleHandler : IRequestHandler<AddRoleCommand, bool>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public AddRoleHandler(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<bool> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(request.Name.ToLower()));
                if (!result.Succeeded)
                {
                    throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Vai trò đã tồn tại.");
                }

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
