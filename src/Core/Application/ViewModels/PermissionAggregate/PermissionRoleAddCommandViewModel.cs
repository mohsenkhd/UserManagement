using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.PermissionAggregate
{
    public class PermissionRoleAddCommandViewModel
    {
        [Required(ErrorMessage = "پر کردن {0} الزامی است")]
        public long RoleId { get; set; }

        [Required(ErrorMessage = "پر کردن {0} الزامی است")]
        public List<long> PermissionIds { get; set; } = null!;
    }
}