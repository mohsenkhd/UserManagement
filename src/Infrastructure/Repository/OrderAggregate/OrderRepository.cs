using Application.RepositoryContracts.OrderAggregate;
using Common.Exceptions.UserManagement;
using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Repository.OrderAggregate
{
    public class OrderRepository : IOrderRepository
    {
        private readonly UserManagementContext _context;
        IQueryable<Order> AllOrders;

        public OrderRepository(UserManagementContext dbContext)
        {
            _context = dbContext;
            AllOrders = dbContext.Orders;
        }
        public async Task<Order> AddOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<Order>> GetOrders()
        {
            return await _context.Orders.Where(a => a.IsDeleted == false).ToListAsync();
        }

        public async Task<List<Order>> GetOrdersByUserId(long userId)
        {
            return await _context.Orders.Where(a => a.IsDeleted == false && a.UserId == userId).ToListAsync();
        }

        public async Task<Order> GetOrderById(long orderId)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(a => a.Id == orderId && a.IsDeleted == false);
            return order ?? throw UserManagementExceptions.OrderNotFoundException;
        }
    }
}
