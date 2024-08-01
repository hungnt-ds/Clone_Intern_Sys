using AutoMapper;
using InternSystem.Application.Common.Constants;
using InternSystem.Application.Common.Persistences.IRepositories;
using InternSystem.Application.Features.AuthManagement.UserManagement.Commands;
using InternSystem.Application.Features.AuthManagement.UserManagement.Models;
using InternSystem.Application.Features.ProjectAndTechnologyManagement.DuAnManagement.Models;
using InternSystem.Domain.BaseException;
using InternSystem.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InternSystem.Application.Features.AuthManagement.UserManagement.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateUserCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, UserManager<AspNetUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (await _userManager.Users.AnyAsync(x => x.UserName == request.Username))
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.DUPLICATE, "User đã tồn tại.");

                var roleName = request.RoleName.Trim();
                IdentityRole? role = await _roleManager.FindByNameAsync(roleName);

                if (role == null)
                    throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Vai trò không tồn tại.");

                if (request.InternInfoId != null)
                {
                    if (await _unitOfWork.InternInfoRepository.GetByIdAsync(request.InternInfoId) == null)
                        throw new ArgumentNullException(nameof(request.InternInfoId), "Không tìm thấy thông tin thực tập sinh");

                    var repository = _unitOfWork.GetRepository<AspNetUser>();
                    var userQuery = repository.GetAllQueryable();

                    var existingUserWithInternId = await repository.ToListAsync(
                        userQuery.Where(c => c.InternInfoId == request.InternInfoId && !c.IsDelete),
                        cancellationToken
                    );

                    if (existingUserWithInternId.Any())
                    {
                        throw new ErrorException(StatusCodes.Status409Conflict, ResponseCodeConstants.BADREQUEST, "Intern đã có tài khoản");
                    }
                }

                AspNetUser newUser = _mapper.Map<AspNetUser>(request);

                IdentityResult resultCreateUser = await _userManager.CreateAsync(newUser, request.Password);
                if (!resultCreateUser.Succeeded)
                    throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra");

                IdentityResult resultAddRole = await _userManager.AddToRoleAsync(newUser, role.Name!);
                if (!resultAddRole.Succeeded)
                    throw new ErrorException(StatusCodes.Status500InternalServerError, ResponseCodeConstants.INTERNAL_SERVER_ERROR, "Đã có lỗi xảy ra");

                return _mapper.Map<CreateUserResponse>(newUser);
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
