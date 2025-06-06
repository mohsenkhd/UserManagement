using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepositoryContracts.InvoiceAggregate
{
    public interface IInvoiceRepository
    {
        Task<Invoice> AddInvoice(Invoice invoice);
        Task<List<Invoice>> GetInvoices();
        Task<List<Invoice>> GetInvoicesByUserId(long userId);
        Task<Invoice> GetInvoiceById(long invoiceId);
        Task<Invoice> UpdateInvoice(Invoice model);
    }
}
