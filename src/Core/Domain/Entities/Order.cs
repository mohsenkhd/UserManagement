using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Order : Domain.Base.Base
    {
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}