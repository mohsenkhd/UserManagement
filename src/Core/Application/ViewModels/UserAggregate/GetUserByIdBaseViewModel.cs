using Application.ViewModels.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.UserAggregate
{
    public class GetUserByIdBaseViewModel : MainRes
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Phone { get; set; } = null!;
        public string? NationalCode { get; set; }
        public long? CustomerNumber { get; set; }
    }
}
