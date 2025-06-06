using Application.ViewModels.AccountAggregate;
using Application.ViewModels.UserAggregate;
using Domain.Entities;
using System.Threading.Tasks;


namespace Application.ServiceContracts.UserAggregate
{
    public interface IUserService
    {
        Task<RegisterUserBaseViewMode> Register(RegisterUserAddCommandModel register);
        Task<RegisterCompletedBaseViewModel> RegisterCompleted(RegisterCompletedAddCommandModel model);
        Task<TokenResponseBaseViewModel> LoginWithMobileNumberAndPassword(LoginWithMobileNumberAndPasswordAddCommandModel login);
        Task<LoginWithNationalCodeBaseViewModel> LoginWithNationalCode(LoginWithNationalCodeAddCommandModel login);
        Task<TokenResponseBaseViewModel> CompleteLoginWithNationalCode(CompleteLoginWithNationalCode model);
        Task<bool> CheckUserPermissionAsync(CheckUserPermissionAddCommandModel model);
        Task<GetRolesAndUserIdFromTokenBaseViewModel> GetRolesAndUserIdFromTokenAsync(string tokenString);
        Task<GetUsersInfoBaseViewModel> GetUsersInfo(GetUsersInfoAddCommandModel model);
        Task<GetUsersWithRolesBaseViewModel> GetUsersWithRoles(GetUsersWithRolesAddCommandModel req);
        Task<AdduserBaseViewModel> AddUser(AddUserCommandModel req);
        Task<GetUserWithRoleBaseViewModel> GetUserWithRoles(GetUserWithRoleCommandModel req);
        Task<UpdatePasswordBaseViewModel> UpdatePassword(UpdatePasswordCommandModel req);
        Task<UpdateUserWithRolesBaseViewModel> UpdateUserWithRoles(UpdateUserWithRolesCommandModel req, long UserId,List<string> permissionIds);
        Task<ActiveDeductiveUserViewModel> ActiveInactiveUser(ActiveDeductiveUserCommandModel model);
        Task<GenerateTokenWithRefreshTokenBaseViewModel> GenerateTokenWithRefreshToken(GenerateTokenWithRefreshTokenCommandModel req);
        Task<GetUserByIdBaseViewModel> GetUserById( GetUserByIdCommandModel req);
        Task<List<GetUsersLoginHistoryForBiBaseViewModel>> GetUsersLoginHistoryForBi(GetUsersLoginHistoryForBiAddCommandModel model);
        Task SendOtp(SendOtpAddCommandModel model);
        Task<GetUsersForBiBaseViewModel> FilterForBiAsync(GetUsersForBiAddCommandModel model);
        Task<ForgotPasswordUserInformationViewModel> ForgotPasswordUserInformation(ForgotPasswordUserInformationCommandModel model);
        Task<GetOtpForForgotPasswordViewModel> GetOtpForForgotPassword(GetOtpForForgotPasswordCommandModel model);
        Task<ChangeForgotPasswordViewModel> ChangeForgotPassword(ChangeForgotPasswordCommandModel model);
        Task<UserCountReportViewModel> UserCountReport();
        Task<GetUserByCustomerNumberBaseViewModel> GetUserByCustomerNumber(GetUserByCustomerNumberCommandModel req);

    }
}