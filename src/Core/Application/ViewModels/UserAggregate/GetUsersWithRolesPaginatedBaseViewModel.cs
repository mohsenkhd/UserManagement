using Application.ViewModels.RoleAggregate;
using Domain.Entities;

namespace Application.ViewModels.UserAggregate
{
    public class GetUsersWithRolesPaginatedBaseViewModel
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

        public static explicit operator GetUsersWithRolesPaginatedBaseViewModel(User v)
        {
            return new GetUsersWithRolesPaginatedBaseViewModel
            {
                UserId = v.Id,
                Email = v.Email,
                FirstName = v.FirstName,
                LastName = v.LastName,
                IsActive = v.IsActive,
                MobileNumber = v.Phone,
                NationalCode = v.NationalCode,
                CreateDate = v.CreatedAt,
                UserRoles = v.UserRoles.Select(ur => new RolesOfUserBaseViewModel
                {
                    RoleName = ur.Role.Name,
                    RoleDescription = ur.Role.Description,
                    RoleId = ur.Role.Id
                }).ToList()
            };
        }

        public static explicit operator GetUsersWithRolesPaginatedBaseViewModel(GetUserWithRoleBaseViewModel v)
        {
            return new GetUsersWithRolesPaginatedBaseViewModel
            {
                UserId = v.UserId,
                CreateDate = v.CreateDate,
                Email = v.Email,
                FirstName = v.FirstName,
                LastName = v.LastName,
                IsActive = v.IsActive,
                MobileNumber = v.MobileNumber,
                NationalCode = v.NationalCode,
                UserRoles = v.UserRoles

            };
        }
    }
}