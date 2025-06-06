using Application.ViewModels.Main;

namespace Application.ViewModels.AccountAggregate;

public class GetTokenForBranchesViewModel : MainRes
{
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;

}
