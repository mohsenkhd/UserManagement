using Application.ViewModels.AccountAggregate;
using Application.ViewModels.InvoiceAggregate;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceContracts.OrderAggregate
{
    public interface IOrderService
    {
        Task<List<OrderViewModel>> GetAllOrdersForAdmin();
        Task<List<OrderViewModel>> GetOrdersForCustomer(long userId);
        Task<OrderViewModel> RegisterOrder(OrderAddCommand command);
    }
}
