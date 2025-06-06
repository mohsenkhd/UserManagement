using Application.RepositoryContracts.OrderAggregate;
using Application.ServiceContracts.InvoiceAggregate;
using Application.ServiceContracts.OrderAggregate;
using Application.ViewModels.AccountAggregate;
using Application.ViewModels.InvoiceAggregate;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.OrderAggregate
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IInvoiceService _invoiceService;

        public OrderService(IOrderRepository orderRepo,IInvoiceService invoiceService)
        {
            _orderRepo = orderRepo;
            _invoiceService = invoiceService;
        }

        public async Task<OrderViewModel> RegisterOrder(OrderAddCommand command)
        {
            var order = new Order
            {
                Product = command.Product,
                UserId = command.UserId,
                OrderDate = DateTime.Now,
                Quantity = command.Quantity,
                TotalAmount = command.TotalAmount
            };

            await _orderRepo.AddOrder(order);

            InvoiceAddCommand invoiceAddModel = new InvoiceAddCommand
            {
                OrderId = order.Id,
                UserId = command.UserId,
                DueDate = DateTime.Now
            };
            var invoice = _invoiceService.CreateInvoice(invoiceAddModel);

            return new OrderViewModel
            {
                OrderId = order.Id,
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

        public async Task<List<OrderViewModel>> GetAllOrdersForAdmin()
        {
            var res = new List<OrderViewModel>();
            var orders = await _orderRepo.GetOrders();
            orders = orders.OrderByDescending(i => i.OrderDate).ToList();

            foreach (var item in orders)
            {
                res.Add((OrderViewModel)item);
            }

            return res;
        }

        public async Task<List<OrderViewModel>> GetOrdersForCustomer(long userId)
        {
            var res = new List<OrderViewModel>();
            var orders = await _orderRepo.GetOrdersByUserId(userId);
            orders = orders.OrderByDescending(i => i.OrderDate).ToList();

            foreach (var item in orders)
            {
                res.Add((OrderViewModel)item);
            }

            return res;
        }
    }
}
