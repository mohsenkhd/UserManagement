using Application.ViewModels.Main;
using Application.ViewModels.RoleAggregate;


namespace Application.ViewModels.UserAggregate
{
    public class GetUserWithRoleBaseViewModel : MainRes
    {
        public long UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string MobileNumber { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? NationalCode { get; set; }
        public string? Email { get; set; }
        public DateTime CreateDate { get; set; }
        public List<RolesOfUserBaseViewModel> UserRoles { get; set; } = null!;
    }
}