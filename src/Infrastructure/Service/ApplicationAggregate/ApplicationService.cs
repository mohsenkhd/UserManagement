using Application.RepositoryContracts.ApplicationAggregate;
using Application.RepositoryContracts.RoleAggregate;
using Application.RepositoryContracts.UserAggregate;
using Application.ServiceContracts.ApplicationAggregate;
using Application.ServiceContracts.PermissionService;
using Application.ServiceContracts.UserAggregate;
using Application.ViewModels.AccountAggregate;
using Application.ViewModels.RoleAggregate;
using Common.Exceptions.UserManagement;
using Common.Helper;
using Domain.Entities;

namespace Service.ApplicationAggregate
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPermissionService _permissionService;
        private readonly IJwtService _jwtService;

        private readonly IPasswordHelper _passwordHelper;
        public ApplicationService(IApplicationRepository applicationRepository, IJwtService jwtService, IRoleRepository roleRepository, IUserRepository userRepository, IPasswordHelper passwordHelper, IPermissionService permissionService)
        {
            _applicationRepository = applicationRepository;
            _passwordHelper = passwordHelper;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtService = jwtService;
            _permissionService = permissionService;
        }
        public async Task<GetTokenViewModel> ApplicationLogin(GetTokenCommandModel model)
        {
            var app = await _applicationRepository.FindByName(model.AppName);
            var verify = _passwordHelper.VerifyPassword(model.Password, app.Password);
            if (!verify)
            {
                throw UserManagementExceptions.InvalidLoginException;
            }
            var user = await _userRepository.GetUserByMobileNumber(model.UserInfo.Phone);
            if (user == null || user.ApplicationFk != app.Id)
            {
                var costumerNumber = model.UserInfo.Phone;
                costumerNumber.Substring(costumerNumber.Length - 10, 10);
                long a;
                var userModel = new User()
                {
                    Phone = model.UserInfo.Phone,
                    ApplicationFk = app.Id,
                    FirstName = model.UserInfo.FirstName,
                    LastName = model.UserInfo.LastName,
                    IsActive = true,
                    OtpConfirm = true,
                    IsDeleted = false,
                    RegisterComplete = true,
                    CustomerNumber = long.Parse(costumerNumber)

                };
                var registeredUser = await _userRepository.RegisterUser(userModel);
                var roleToUserModel = new RoleToUserAddCommandModel()
                {
                    UserId = registeredUser.Id,
                    RoleIds = new List<long> { 2 }
                };
                var addNormalUser = await _roleRepository.AddNormalUserRoleToUser(roleToUserModel);
                if (!addNormalUser.IsSucced)
                {
                    throw UserManagementExceptions.CantAssignRoleToUserException;
                }
                var refreshTokenModel = new GenerateRefreshTokenAddCommandModel()
                {
                    UserId = registeredUser.Id,
                };
                var token = _jwtService.GenerateRefreshToken(refreshTokenModel);
                return new GetTokenViewModel()
                {
                    Token = token,
                };

            }
            var refreshTokenForUserModel = new GenerateRefreshTokenAddCommandModel()
            {
                UserId = user.Id,
            };
            var tokenForUser = _jwtService.GenerateRefreshToken(refreshTokenForUserModel);
            return new GetTokenViewModel()
            {
                Token = tokenForUser,
            };

        }

        public async Task<GetTokenForBranchesViewModel> BranchesApplicationLogin(GetTokenForBranchesCommandModel model)
        {
            var app = await _applicationRepository.FindByName(model.AppName);
            var verify = _passwordHelper.VerifyPassword(model.Password, app.Password);
            if (!verify)
            {
                throw UserManagementExceptions.InvalidLoginException;
            }

            var permissionIds = await _permissionService.GetPermissionsByRoleName("BranchesUser");

            var generateToken = new FullTokenResponseCommandModel()
            {
                PermissionIds = permissionIds.Permissions,
                UserId = 0,
                ApplicationId = app.Id,
                FirstName = string.Empty,
                LastName = string.Empty,
                Phone = string.Empty,
                IsAdmin = false,
                CustomerNumber = 0
            };

            var token = _jwtService.GenerateFullToken(generateToken);
            return new GetTokenForBranchesViewModel()
            {
                Token = token.Token,
                RefreshToken = token.RefreshToken,
            };

        }
    }
}
