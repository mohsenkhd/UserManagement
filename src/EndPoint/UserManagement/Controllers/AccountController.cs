using Application.ServiceContracts.ApplicationAggregate;
using Application.ServiceContracts.Otp;
using Application.ServiceContracts.UserAggregate;
using Application.ViewModels.AccountAggregate;
using Application.ViewModels.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Attributes;
using UserManagement.Filters;


namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Transactional]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;


        public AccountController(IUserService userService, IApplicationService applicationService, IOtpService otpService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<RegisterUserBaseViewMode> Register([FromBody] RegisterUserAddCommandModel model)
        {
            var res = await _userService.Register(model);

            return res;
        }

        [HttpGet("send-otp")]
        public async Task SendOtp([FromQuery]SendOtpAddCommandModel model)
        {
            var sendModel = new SendOtpAddCommandModel()
            {
                UserId = model.UserId,
                Title = model.Title,
            };

            await _userService.SendOtp(sendModel);
        }

        [HttpPost("complete-register")]
        public async Task<RegisterCompletedBaseViewModel> CompleteRegister([FromBody] RegisterCompletedAddCommandModel model)
        {
            var res = await _userService.RegisterCompleted(model);
            return res;
        }

        [HttpPost("login-with-mobile-number-and-password")]
        public async Task<TokenResponseBaseViewModel> LoginWithMobileNumberAndPassword(
            [FromBody] LoginWithMobileNumberAndPasswordAddCommandModel model)
        {
            var result = await _userService.LoginWithMobileNumberAndPassword(model);
            return result;
        }

        [HttpPost("login-with-national-code")]
        public async Task<LoginWithNationalCodeBaseViewModel> LoginWithNationalCode([FromBody] LoginWithNationalCodeAddCommandModel model)
        {
            var result = await _userService.LoginWithNationalCode(model);
            return result;
        }

        [HttpPost("complete-login-with-national-code")]
        public async Task<TokenResponseBaseViewModel> CompleteLoginWithNationalCode([FromBody] CompleteLoginWithNationalCode model)
        {
            var result = await _userService.CompleteLoginWithNationalCode(model);
            return result;
        }
        [HttpPost("check-user-permission")]
        public async Task<bool> CheckUserPermission([FromBody] CheckUserPermissionAddCommandModel model)
        {
            var result = await _userService.CheckUserPermissionAsync(model);
            return result;
        }

        [HttpPost("users-info")]
        [PermissionChecker(196)]
        public async Task<GetUsersInfoBaseViewModel> UsersInfo(GetUsersInfoAddCommandModel model) 
        {
            var result = await _userService.GetUsersInfo(model);
            return result;
        }
        [HttpPost("generate-token-with-refresh-token")]
        public async Task<GenerateTokenWithRefreshTokenBaseViewModel> GenerateTokenWithRefreshToken([FromBody]GenerateTokenWithRefreshTokenCommandModel model)
        {
            var result = await _userService.GenerateTokenWithRefreshToken(model);
            return result;
        }
        [HttpPost("forgot-password-user-information")]
        public async Task<ForgotPasswordUserInformationViewModel> ForgotPasswordUserInformation([FromBody] ForgotPasswordUserInformationCommandModel model)
        {
            var result = await _userService.ForgotPasswordUserInformation(model);
            return result;
        }
        [HttpPost("get-otp-for-forgot-password")]
        public async Task<GetOtpForForgotPasswordViewModel> GetOtpForForgotPassword([FromBody] GetOtpForForgotPasswordCommandModel model)
        {
            var result = await _userService.GetOtpForForgotPassword(model);
            return result;
        }
        [HttpPost("change-forgot-password")]
        public async Task<ChangeForgotPasswordViewModel> ChangeForgotPassword([FromBody] ChangeForgotPasswordCommandModel model)
        {
            var result = await _userService.ChangeForgotPassword(model);
            return result;
        }
   
    }
}