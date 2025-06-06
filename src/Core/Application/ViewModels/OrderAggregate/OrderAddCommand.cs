using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.InvoiceAggregate
{
    public class OrderAddCommand
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public long UserId { get; set; }
    }
}
