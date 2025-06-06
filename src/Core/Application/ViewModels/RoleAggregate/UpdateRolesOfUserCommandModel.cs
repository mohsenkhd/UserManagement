using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.RoleAggregate
{
    public class UpdateRolesOfUserCommandModel
    {
        [Required(ErrorMessage = "پر کردن {0} فیلد الزامی است")]
        public long UserId { get; set; }

        [Required(ErrorMessage = "پر کردن {0} فیلد الزامی است")]
        public List<long> RoleIds { get; set; } = null!;
    }
}