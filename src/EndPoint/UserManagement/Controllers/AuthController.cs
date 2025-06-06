using Application.ServiceContracts.ApplicationAggregate;
using Application.ViewModels.AccountAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserManagement.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        public AuthController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpPost("get-token")]
        public async Task<GetTokenViewModel> GetToken(GetTokenCommandModel model)
        {
            var res = await _applicationService.ApplicationLogin(model);
            return res;
        }

        [HttpPost("get-token-branches")]
        public async Task<GetTokenForBranchesViewModel> GetTokenForBranches(GetTokenForBranchesCommandModel model)
        {
            var res = await _applicationService.BranchesApplicationLogin(model);
            return res;
        }
    }
}
