using Application.ViewModels.Main;

namespace Application.ViewModels.UserAggregate
{
    public class GetUsersWithRolesBaseViewModel : MainRes
    {
        public int Page { get; set; }
        public bool HasNext { get; set; }


        public bool HasPrev { get; set; }


        public int TotalPages { get; set; }


        public long TotalElements { get; set; }

        public List<GetUsersWithRolesPaginatedBaseViewModel> Elements { get; set; } = null!;
    }
}