using Application.ViewModels.AccountAggregate;
using Application.ViewModels.RoleAggregate;
using Application.ViewModels.UserAggregate;
using Domain.Entities;


namespace Application.RepositoryContracts.UserAggregate
{
    public interface IUserRepository
    {
        Task<User> RegisterUser(User user);
        Task<User> GetById(long userId);
        Task<List<User>> GetByIds(List<long> ids);
        Task<bool> CheckUserIsAdmin(long userId);
        User UpdateUser(User user);
        Task<User?> GetUserByMobileNumber(string mobileNumber);
        Task<User?> GetUserByNationalCode(string nationalCode);
        Task<List<string>> GetUserRoleNames(long userId);
        Task<bool> CheckUserPermissionAsync(GetRolesAndUserIdFromTokenAsyncAddCommandModel model);
        Task<List<long>> GetUserPermissionIds(GetUserPermissionIdsCommandModel model);
        Task<GetUsersWithRolesBaseViewModel> GetUsersWithRoles(int page, int pageSize);
        Task<int> FilterAllAsync(UserFilterCommandModel model);
        Task<GetUserWithRoleBaseViewModel> GetUserWithRols( GetUserWithRoleCommandModel req);
        Task<List<UserLoginHistory>> GetUsersLoginHistoryForBi(GetUsersLoginHistoryForBiAddCommandModel model);
        Task<List<User>> FilterForBiAsync(GetUsersForBiAddCommandModel model);
        Task<int> GetTotalUserCountAsync();
        Task<int> GetUsersCreatedLastMonthAsync();
        Task<int> GetUsersCreatedLastWeekAsync();
        Task<int> GetUsersCreatedYesterdayAsync();
        Task<List<long>> GetUsersWithRoleId(long roleId);
        Task<User> GetByCustomerNumber(long customerNumber);
    }
}