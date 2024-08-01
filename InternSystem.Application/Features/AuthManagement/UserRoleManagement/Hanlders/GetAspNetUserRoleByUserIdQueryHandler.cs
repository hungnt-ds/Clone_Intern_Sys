using InternSystem.Application.Common.Constants;
using InternSystem.Application.Features.AuthManagement.UserRoleManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Application.Features.AuthManagement.UserRoleManagement.Hanlders
{
    public class GetAspNetUserRoleByUserIdQueryHandler : IRequestHandler<GetAspNetUserRoleByUserIdQuery, List<string>>
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetAspNetUserRoleByUserIdQueryHandler(UserManager<AspNetUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<string>> Handle(GetAspNetUserRoleByUserIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                {
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy người dùng");
                }

                var roles = await _userManager.GetRolesAsync(user);
                return roles.ToList();
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
