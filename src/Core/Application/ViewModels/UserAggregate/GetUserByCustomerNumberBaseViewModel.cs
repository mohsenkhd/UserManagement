using Application.ViewModels.Main;

namespace Application.ViewModels.UserAggregate;

public class GetUserByCustomerNumberBaseViewModel : MainRes
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
