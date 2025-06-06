using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.RoleAggregate
{
    public class RoleAddCommandModel
    {
        public string? Description { get; set; }
        public string Name { get; set; } = null!;

      
        public List<long> PermissionsIds { get; set; } = null!;
    }
}