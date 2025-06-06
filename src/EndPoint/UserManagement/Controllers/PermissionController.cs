using Application.ServiceContracts.PermissionService;
using Application.ViewModels.PermissionAggregate;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Attributes;
using UserManagement.Filters;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Transactional]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        public PermissionController(IPermissionService permissionService)
        {
                _permissionService = permissionService;
        }

        [HttpPost("add-permissions-to-role")]
        [PermissionChecker(177)]
        public async Task<PermissionToRoleBaseViewModel> AddPermissionsToRole(PermissionRoleAddCommandViewModel model)
        {
            var result = await _permissionService.AddPermissionsToRole(model);
            return result;
        }
        [HttpGet("get-permissions")]
        [PermissionChecker(178)]
        public async Task<PermissionBaseViewModel> GetPermissions()
        {
            var result = await _permissionService.GetAllPermissions();
            return result;
        }
        [HttpPut("update-permissions-of-role")]
        [PermissionChecker(179)]
        public async Task<UpdatePermissionsOfRoleBaseViewModel> UpdatePermissionsOfRole([FromBody] UpdatePermissionsOfRoleCommandModel model)
        {
            var res = await _permissionService.UpdatePermissionsOfRole(model);
            return res;
        }
    }
}
