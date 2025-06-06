using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class HubLog 
    {
        [Key]
        public long Id { get; set; }
        public string HubId { get; set; } = null!;
        public long UserId { get; set; }
        public HubType hubType { get; set; }
    }
    public enum HubType
    {
        Open = 1,
        Close = 2,
    }

}
