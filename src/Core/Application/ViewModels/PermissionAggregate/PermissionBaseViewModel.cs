using Application.ViewModels.Main;
using Domain.Entities;

namespace Application.ViewModels.PermissionAggregate
{
    public class PermissionBaseViewModel : MainRes
    {
        public List<PermissionResultViewModel> Permissions { get; set; } = null!;
    }
}