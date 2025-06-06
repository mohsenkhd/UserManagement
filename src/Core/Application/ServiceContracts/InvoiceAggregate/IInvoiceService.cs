using Application.ViewModels.AccountAggregate;
using Application.ViewModels.InvoiceAggregate;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceContracts.InvoiceAggregate
{
    public interface IInvoiceService
    {
        Task<InvoiceViewModel> CreateInvoice(InvoiceAddCommand command);
        Task<List<InvoiceViewModel>> GetInvoicesForCustomer(long userId);
        Task<List<InvoiceViewModel>> GetAllInvoicesForAdmin();
    }
}
