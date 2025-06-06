using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.PermissionAggregate
{
    public class UpdatePermissionsOfRoleCommandModel
    {
        [Required(ErrorMessage = "پر کردن {0} فیلد الزامی است")]
        public long RoleId { get; set; }

        [Required(ErrorMessage = "پر کردن {0} فیلد الزامی است")]
        public List<long> PermissionsIds { get; set; } = null!;
    }
}