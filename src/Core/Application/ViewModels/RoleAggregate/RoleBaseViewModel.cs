using Application.ViewModels.Main;
using Domain.Entities;

namespace Application.ViewModels.RoleAggregate
{
    public class RoleBaseViewModel : MainRes
    {
        public long RoleId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public static explicit operator RoleBaseViewModel(Role v)
        {
            return new RoleBaseViewModel()
            {
                Description = v.Description,
                Name = v.Name,
                RoleId = v.Id
            };
        }
    }
}