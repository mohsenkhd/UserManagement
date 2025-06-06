using Domain.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class UserLoginHistory
    {
        [Key]
        public long Id { get; set; }
        public long UserFk { get; set; }
        public DateTime LogDate { get; set; }
        public UserHistoryType HistoryType { get; set; }
        public long? CustomerNumber { get; set; }

    }
    public enum UserHistoryType
    {
        Login = 1,
        Logout = 2,
    }
}
