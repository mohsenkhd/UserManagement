using Application.ViewModels.Main;
using Domain.Entities;

namespace Application.ViewModels.AccountAggregate
{
    public class GetUsersInfoBaseViewModel :MainRes
    {
        public List<UserInfoViewModel> UsersInfo { get; set;} = new List<UserInfoViewModel>();

    }
}
