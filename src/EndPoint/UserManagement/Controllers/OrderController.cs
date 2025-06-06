using Application.ServiceContracts.OrderAggregate;
using Application.ViewModels.AccountAggregate;
using Application.ViewModels.InvoiceAggregate;
using Application.ViewModels.RoleAggregate;
using Application.ViewModels.UserAggregate;
using Microsoft.AspNetCore.Mvc;
using Service.OrderAggregate;
using UserManagement.Attributes;
using UserManagement.Filters;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Transactional]
    public class OrderControllerr : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderControllerr(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("add-order")]
        [PermissionChecker(351)]
        public async Task<OrderViewModel> AddOrder([FromBody] OrderAddCommand req)
        {
            var result = await _orderService.RegisterOrder(req);
            return result;
        }

        [HttpGet("get-orders-customer")]
        [PermissionChecker(345)]
        public async Task<List<OrderViewModel>> GetCustomerOrders()
        {
            var userToken = HttpContext.Items["UserToken"] as DecodeToken;
            var userId = long.Parse(userToken.UserId);
            var res = await _orderService.GetOrdersForCustomer(userId);
            return res;
        }

        [HttpGet("get-orders")]
        [PermissionChecker(346)]
        public async Task<List<OrderViewModel>> GetOrders()
        {
            var res = await _orderService.GetAllOrdersForAdmin();
            return res;
        }
    }
}
