using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.ApplicationAggregate
{
    public class ApplicationLoginCommandModel
    {
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
    }
}
