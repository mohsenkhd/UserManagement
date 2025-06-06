using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserAggregate
{
    public class UserForBiResponse
    {
        public long UserId { get; set; }
        public long? CostumerNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string MobileNumber { get; set; } = null!;
        public bool IsActive { get; set; }
        public string? NationalCode { get; set; }
        public string? Email { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
