using Application.ServiceContracts.InvoiceAggregate;
using Application.ServiceContracts.WalletAggregate;
using Application.ViewModels.AccountAggregate;
using Application.ViewModels.InvoiceAggregate;
using Application.ViewModels.WalletAggregate;
using Microsoft.AspNetCore.Mvc;
using Service.InvoiceAggregate;
using UserManagement.Attributes;
using UserManagement.Filters;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Transactional]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IWalletService _walletService;
        public InvoiceController(IInvoiceService invoiceService, IWalletService walletService)
        {
            _invoiceService = invoiceService;
            _walletService = walletService;
        }

        [HttpGet("get-invoices-customer")]
        [PermissionChecker(345)]
        public async Task<List<InvoiceViewModel>> GetCustomerInvoices()
        {
            var userToken = HttpContext.Items["UserToken"] as DecodeToken;
            var userId = long.Parse(userToken.UserId);
            var res = await _invoiceService.GetInvoicesForCustomer(userId);
            return res;
        }

        [HttpGet("get-invoices")]
        [PermissionChecker(346)]
        public async Task<List<InvoiceViewModel>> GetInvoices()
        {
            var res = await _invoiceService.GetAllInvoicesForAdmin();
            return res;
        }

        [HttpPost("add-order")]
        [PermissionChecker(350)]
        public async Task<bool> PayInvoice([FromBody] WalletPaymentModel req)
        {
            var userToken = HttpContext.Items["UserToken"] as DecodeToken;
            var userId = long.Parse(userToken.UserId);

            if (req.UserId != userId)
                throw new Exception("شناسه مشتری با فاکتور مغایرت دارد");
            
            var result = await _walletService.PayInvoice(req.UserId , req.InvoiceId);
            return result;
        }
    }
}
