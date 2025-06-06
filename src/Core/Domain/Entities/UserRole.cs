using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserRole: Domain.Base.Base
    {
        public long UserId { get; set; }

        public long RoleId { get; set; }


        public User User { get; set; } = null!;

        public Role Role { get; set; } = null!;
    }
}
