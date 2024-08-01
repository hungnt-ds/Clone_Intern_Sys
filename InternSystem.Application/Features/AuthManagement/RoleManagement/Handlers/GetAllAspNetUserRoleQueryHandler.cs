using InternSystem.Application.Common.Constants;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Models;
using InternSystem.Application.Features.AuthManagement.RoleManagement.Queries;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace InternSystem.Application.Features.AuthManagement.RoleManagement.Handlers
{
    public class GetAllAspNetUserRoleQueryHandler : IRequestHandler<GetAllAspNetUserRoleQuery, List<UserRoleDto>>
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GetAllAspNetUserRoleQueryHandler(UserManager<AspNetUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<UserRoleDto>> Handle(GetAllAspNetUserRoleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allUserRoles = new List<UserRoleDto>();
                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        var roleEntity = await _roleManager.FindByNameAsync(role);
                        if (roleEntity != null)
                        {
                            allUserRoles.Add(new UserRoleDto { UserId = user.Id, RoleId = roleEntity.Id, HoVaTen = user.HoVaTen, RoleName = roleEntity.Name });
                        }
                    }
                }
                return allUserRoles;
            }
            catch (ErrorException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra");
            }
        }
    }
}
