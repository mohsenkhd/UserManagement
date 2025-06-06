using Application.ViewModels.AccountAggregate;

namespace Application.ServiceContracts.ApplicationAggregate;

public interface IApplicationService
{
    Task<GetTokenViewModel> ApplicationLogin(GetTokenCommandModel model);
    Task<GetTokenForBranchesViewModel> BranchesApplicationLogin(GetTokenForBranchesCommandModel model);
}
