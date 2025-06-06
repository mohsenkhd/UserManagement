using Application.RepositoryContracts.InvoiceAggregate;
using Application.RepositoryContracts.OrderAggregate;
using Application.ServiceContracts.InvoiceAggregate;
using Application.ViewModels.AccountAggregate;
using Application.ViewModels.InvoiceAggregate;
using Application.ViewModels.RoleAggregate;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.InvoiceAggregate
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IInvoiceRepository _invoiceRepo;

        public InvoiceService(IOrderRepository orderRepo, IInvoiceRepository invoiceRepo)
        {
            _orderRepo = orderRepo;
            _invoiceRepo = invoiceRepo;
        }

        public async Task<InvoiceViewModel> CreateInvoice(InvoiceAddCommand command)
        {
            var order = await _orderRepo.GetOrderById(command.OrderId);
            if (order == null || order.UserId != command.UserId)
                throw new Exception("Order not found or does not belong to the customer.");

            var invoice = new Invoice
            {
                OrderId = command.OrderId,
                Amount = order.TotalAmount,
                DueDate = command.DueDate,
                Status = InvoiceStatus.Pending
            };

            await _invoiceRepo.AddInvoice(invoice);
            return new InvoiceViewModel
            {
                InvoiceId = invoice.Id,
                OrderId = invoice.OrderId,
                Amount = invoice.Amount,
                DueDate = invoice.DueDate,
                Status = invoice.Status,
                Product = order.Product,
                Quantity = order.Quantity,
                TotalAmount = order.TotalAmount,
                OrderDate = order.OrderDate,
                UserId = order.UserId,
                FirstName = order.User.FirstName,
                LastName = order.User.LastName,
                Phone = order.User.Phone,
                NationalCode = order.User.NationalCode,
                Email = order.User.Email,
                CustomerNumber = order.User.CustomerNumber
            };
        }

        public async Task<List<InvoiceViewModel>> GetAllInvoicesForAdmin()
        {
            var res = new List<InvoiceViewModel>();
            var invoices = await _invoiceRepo.GetInvoices();
            invoices = invoices.OrderByDescending(i => i.DueDate).ToList();

            foreach (var item in invoices)
            {
                res.Add((InvoiceViewModel)item);
            }

            return res;
        }

        public async Task<List<InvoiceViewModel>> GetInvoicesForCustomer(long userId)
        {
            var res = new List<InvoiceViewModel>();
            var invoices = await _invoiceRepo.GetInvoicesByUserId(userId);
            invoices = invoices.OrderByDescending(i => i.DueDate).ToList();

            foreach (var item in invoices)
            {
                res.Add((InvoiceViewModel)item);
            }

            return res;
        }
    }
}
