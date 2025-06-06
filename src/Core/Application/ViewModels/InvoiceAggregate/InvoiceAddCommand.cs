using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.InvoiceAggregate
{
    public class InvoiceAddCommand
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public DateTime DueDate { get; set; }
    }
}
