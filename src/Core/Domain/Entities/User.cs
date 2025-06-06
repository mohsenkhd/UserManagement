using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User : Domain.Base.Base
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Phone { get; set; } = null!;
        public long ApplicationFk { get; set; }
        public bool OtpConfirm { get; set; } = false;
        public bool IsActive { get; set; }
        public string? Password { get; set; }
        public string? NationalCode { get; set; }
        public string? Email { get; set; }
        public bool? EmailConfirm { get; set; }
        public bool RegisterComplete { get; set; }
        public long? CustomerNumber { get; set; }
        [MaxLength(5), MinLength(5)]
        public string? OtpCode { get; set; }
        public DateTime? OtpCodeCreatedTime { get; set; }
        public bool? IsOtpUsed { get; set; }
        public virtual Application? Application { get; set; } = null!;
        public ICollection<UserRole> UserRoles { get; set; } = null!;
        public ICollection<Order> Orders { get; set; }
        public Wallet Wallet { get; set; }
    }
}