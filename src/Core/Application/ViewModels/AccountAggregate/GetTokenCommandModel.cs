using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountAggregate
{
    public class GetTokenCommandModel
    {
        [Required]
        public string AppName { get; set; } = null!;
        [Required]
        public string Password { get; set; }=null!;
        [Required]
        public UserLoginWithAppCommandModel UserInfo { get; set; }=new UserLoginWithAppCommandModel();
    }
}
