using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Invoice : Domain.Base.Base
    {
        public long OrderId { get; set; }
        public Order Order { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Pending;
    }
}