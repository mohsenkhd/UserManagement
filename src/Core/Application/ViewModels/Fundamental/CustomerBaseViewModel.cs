using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Fundamental
{
    public class CustomerBaseViewModel
    {
        public bool isExists { get; set; }
        public bool isActive { get; set; }
        public bool isMobileVerified { get; set; }
        public string firstname { get; set; } = null!;
        public string lastname { get; set; } = null!;
        public long? customerNumber { get; set; }
    }
}
