using Application.ServiceContracts.RoleAggregate;
using Application.ViewModels.AccountAggregate;
using Application.ViewModels.RoleAggregate;
using Application.ViewModels.UserAggregate;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Attributes;
using UserManagement.Filters;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Transactional]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("add-role-with-permissions")]
        [PermissionChecker(180)]
        public async Task<RoleBaseViewModel> AddRolewithpermissions([FromBody] RoleAddCommandModel command)
        {
            var role = await _roleService.AddRole(command);
            return role;
        }

        [HttpPost("add-roles-to-user")]
        [PermissionChecker(181)]
        public async Task<RoleToUserBaseViewModel> AddRolesToUser([FromBody] RoleToUserAddCommandModel model)
        {
            var result = await _roleService.AddRolesToUser(model);
            return result;
        }

        [HttpGet("get-roles-with-permissions")]
        [PermissionChecker(182)]
        public async Task<GetRolesBaseViewModel> GetRolesWithPermissions([FromQuery] GetRolesAddCommandModel model)
        {
            var res = await _roleService.GetRolesWithPermissions(model);
            return res;
        }
        [HttpDelete("delete-role")]
        [PermissionChecker(183)]
        public async Task<DeleteRoleBaseViewModel> DeleteRole(long roleId)
        {
            var res = await _roleService.DeleteRole(roleId);
            return res;
        }
        [HttpPut("update-role-with-permissions")]
        [PermissionChecker(184)]
        public async Task<UpdateRoleBaseViewModel> UpdateRole([FromBody] UpdateRoleCommandModel model)
        {
            var res = await _roleService.UpdateRole(model);
            return res;
        }
        [HttpPut("update-roles-of-user")]
        [PermissionChecker(185)]
        public async Task<UpdateRolesOfUserBaseViewModel> UpdateRolesOfUser([FromBody] UpdateRolesOfUserCommandModel model)
        {
            var res = await _roleService.UpdateRolesOfUser(model);
            return res;
        }
        [HttpPost("active-deactive-role")]
        [PermissionChecker(186)]
        public async Task<ActiveDeductiveRoleViewModel> ActiveInactiveRole([FromBody] ActiveDeductiveRoleCommandModel model)
        {
            var result = await _roleService.ActiveInactiveRole(model);
            return result;
        }
        [HttpPost("get-roles-search")]
        [PermissionChecker(187)]
        public async Task<GetRolesSearchViewModel> GetRolesSearch([FromBody] GetRolesSearchAddCommandModel model)
        {
            var res = await _roleService.GetRolesSearch(model);
            return res;
        }
        [HttpGet("get-role-with-permissions")]
        [PermissionChecker(188)]
        public async Task<GetRoleWithPermissionsByIdBaseViewModel> GetRoleWithPermissionsById([FromQuery] GetRoleWithPermissionsByIdCommandModel model)
        {
            var res = await _roleService.GetRoleWithPermissionsById(model);
            return res;
        }
    }
}