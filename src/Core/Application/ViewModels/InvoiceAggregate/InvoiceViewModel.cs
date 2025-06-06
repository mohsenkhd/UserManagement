using Domain.Entities;
using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.AccountAggregate
{
    public class InvoiceViewModel
    {
        public long InvoiceId { get; set; }
        public long OrderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public InvoiceStatus Status { get; set; }
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

        public static explicit operator InvoiceViewModel(Invoice v)
        {
            return new InvoiceViewModel
            {
                InvoiceId = v.Id,
                OrderId = v.OrderId,
                Amount = v.Amount,
                DueDate = v.DueDate,
                Status = v.Status,
                Product = v.Order.Product,
                Quantity = v.Order.Quantity,
                TotalAmount = v.Order.TotalAmount,
                OrderDate = v.Order.OrderDate,
                UserId = v.Order.UserId,
                FirstName = v.Order.User.FirstName,
                LastName = v.Order.User.LastName,
                Phone = v.Order.User.Phone,
                NationalCode = v.Order.User.NationalCode,
                Email = v.Order.User.Email,
                CustomerNumber = v.Order.User.CustomerNumber
            };
        }
    }
}
