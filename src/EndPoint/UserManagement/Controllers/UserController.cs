using Application.ServiceContracts.ClientAggregate;
using Application.ServiceContracts.UserAggregate;
using Application.ServiceContracts.UsersLoginHistory;
using Application.ServiceContracts.WalletAggregate;
using Application.ViewModels.AccountAggregate;
using Application.ViewModels.ClientAggregate;
using Application.ViewModels.RoleAggregate;
using Application.ViewModels.UserAggregate;
using Application.ViewModels.UserLoginHistory;
using Application.ViewModels.WalletAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Attributes;
using UserManagement.Filters;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Transactional]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IClientService _clientService;
        private readonly IUsersLoginHistoryService _loginHistoryService;
        private readonly IWalletService _walletService;

        public UserController(IUsersLoginHistoryService loginHistoryService, IUserService userService, IClientService clientService,IWalletService walletService)
        {
            _userService = userService;
            _clientService = clientService;
            _loginHistoryService = loginHistoryService;
            _walletService = walletService;
        }
        [HttpPost("get-users-with-roles")]
        [PermissionChecker(189)]
        public async Task<GetUsersWithRolesBaseViewModel> GetUsersWithRoles(GetUsersWithRolesAddCommandModel req)
        {
            var result = await _userService.GetUsersWithRoles(req);
            return result;
        }
        [HttpPost("add-user-with-roles")]
        [PermissionChecker(190)]
        public async Task<AdduserBaseViewModel> AddUserWithRoles([FromBody] AddUserCommandModel req)
        {
            var result = await _userService.AddUser(req);
            return result;
        }
        [HttpGet("get-user-with-roles")]
        [PermissionChecker(191)]
        public async Task<GetUserWithRoleBaseViewModel> GetUserWithRols([FromQuery] GetUserWithRoleCommandModel req)
        {
            var result = await _userService.GetUserWithRoles(req);
            return result;
        }
        [HttpPost("update-password")]
        [PermissionChecker(192)]
        public async Task<UpdatePasswordBaseViewModel> UpdatePassword([FromBody] UpdatePasswordCommandModel req)
        {
            var result = await _userService.UpdatePassword(req);
            return result;
        }
        [HttpPost("update-user-with-roles")]
        [PermissionChecker(193)]
        public async Task<UpdateUserWithRolesBaseViewModel> UpdateUserWithRoles([FromBody] UpdateUserWithRolesCommandModel req)
        {
            var userToken = HttpContext.Items["UserToken"] as DecodeToken;
            var result = await _userService.UpdateUserWithRoles(req,long.Parse(userToken.UserId),userToken.PermissionIds);
            return result;
        }
        [HttpPost("active-deactive-user")]
        [PermissionChecker(194)]
        public async Task<ActiveDeductiveUserViewModel> ActiveInactiveUser([FromBody] ActiveDeductiveUserCommandModel model)
        {
            var result = await _userService.ActiveInactiveUser(model);
            return result;
        }
        [HttpGet("get-user-by-id")]
        [PermissionChecker(195)]
        public async Task<GetUserByIdBaseViewModel> GetUserById([FromQuery] GetUserByIdCommandModel req)
        {
            var result = await _userService.GetUserById(req);
            return result;
        }
        [HttpPost("get-users-login-history-for-bi")]

        public async Task<List<GetUsersLoginHistoryForBiBaseViewModel>> GetUsersLoginHistoryForBi([FromBody] GetUsersLoginHistoryForBiAddCommandModel req)
        {
            var result = await _userService.GetUsersLoginHistoryForBi(req);
            return result;
        }
        [HttpPost("get-users-for-bi")]
        public async Task<GetUsersForBiBaseViewModel> GetUsersForBi([FromBody] GetUsersForBiAddCommandModel req)
        {
            var result = await _userService.FilterForBiAsync(req);
            return result;
        }
        [HttpGet("user-count-report")]
        [PermissionChecker(243)]
        public async Task<UserCountReportViewModel> UserCountReport()
        {
            var res = await _userService.UserCountReport();
            return res;
        }
        [HttpGet("get-client-api-key")]
        public async Task<GetClientViewModel> GetClientApiKey([FromQuery] GetClientCommandModel req)
        {
            var result = await _clientService.GetClient(req);
            return result;
        }
        [HttpPost("get-user-login-history")]
        public async Task<GetUserLoginHistoryBaseViewModel> GetUserLoginHistory(GetUserLoginHistoryAddCommandModel req)
        {
            var res = await _loginHistoryService.GetUserLoginHistory(req);
            return res;
        }

        [HttpGet("get-user-by-customer-number")]
        [PermissionChecker(340)]
        public async Task<GetUserByCustomerNumberBaseViewModel> GetUserByCustomerNumber([FromQuery] GetUserByCustomerNumberCommandModel req)
        {
            var result = await _userService.GetUserByCustomerNumber(req);
            return result;
        }
        
        [HttpGet("get-wallet-by-transactions")]
        [PermissionChecker(352)]
        public async Task<WalletViewModel> GetWalletWithTransactions()
        {
            var userToken = HttpContext.Items["UserToken"] as DecodeToken;
            var userId = long.Parse(userToken.UserId);
            var result = await _walletService.GetWalletTransactionByUserId(userId);
            return result;
        }
    }
}
