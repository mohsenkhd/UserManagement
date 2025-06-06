using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.AccountAggregate
{
    public class OrderViewModel
    {
        public long OrderId { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public long UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Phone { get; set; } = null!;
        public string? NationalCode { get; set; }
        public string? Email { get; set; }
        public long? CustomerNumber { get; set; }

        public static explicit operator OrderViewModel(Order v)
        {
            return new OrderViewModel
            {
                OrderId = v.Id,
                Product = v.Product,
                Quantity = v.Quantity,
                TotalAmount = v.TotalAmount,
                OrderDate = v.OrderDate,
                UserId = v.UserId,
                FirstName = v.User.FirstName,
                LastName = v.User.LastName,
                Phone = v.User.Phone,
                NationalCode = v.User.NationalCode,
                Email = v.User.Email,
                CustomerNumber = v.User.CustomerNumber
            };
        }
    }
}
