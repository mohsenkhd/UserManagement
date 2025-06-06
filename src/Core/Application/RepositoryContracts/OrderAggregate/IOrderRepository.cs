using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepositoryContracts.OrderAggregate
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetOrdersByUserId(long userId);
        Task<List<Order>> GetOrders();
        Task<Order> GetOrderById(long orderId);
        Task<Order> AddOrder(Order order);
    }
}
