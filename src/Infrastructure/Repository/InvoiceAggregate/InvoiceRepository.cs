using Application.RepositoryContracts.InvoiceAggregate;
using Common.Exceptions.UserManagement;
using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.InvoiceAggregate
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly UserManagementContext _context;
        IQueryable<Invoice> AllInvoices;

        public InvoiceRepository(UserManagementContext dbContext)
        {
            _context = dbContext;
            AllInvoices = dbContext.Invoices;
        }
        public async Task<Invoice> AddInvoice(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<List<Invoice>> GetInvoices()
        {
            return await _context.Invoices.Where(a => a.IsDeleted == false).ToListAsync();
        }

        public async Task<List<Invoice>> GetInvoicesByUserId(long userId)
        {
            return await _context.Invoices.Where(a => a.Order.UserId == userId && a.IsDeleted == false).OrderByDescending(x => x.DueDate).ToListAsync();
        }

        public async Task<Invoice> GetInvoiceById(long invoiceId)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(a => a.Id == invoiceId && a.IsDeleted == false);
            return invoice ?? throw UserManagementExceptions.InvoiceNotFoundException;
        }

        public async Task<Invoice> UpdateInvoice(Invoice model)
        {
            var invoice = await GetInvoiceById(model.Id);
            if (invoice == null) throw UserManagementExceptions.InvoiceNotFoundException;

            invoice.Status = model.Status;
            _context.Invoices.Update(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }
    }
}
