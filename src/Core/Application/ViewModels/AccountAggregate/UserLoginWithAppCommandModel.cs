using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.AccountAggregate
{
    public class UserLoginWithAppCommandModel
    {
        public string? FirstName { get; set; }

        /// <summary>
        /// نام خانوادکی کاربر
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// تلفن کاربر
        /// </summary>
        [Required]
        public string Phone { get; set; } = null!;
    }
}
