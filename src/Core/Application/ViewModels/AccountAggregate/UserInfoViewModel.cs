using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.AccountAggregate
{
    public class UserInfoViewModel
    {
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// نام کاربر
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// نام خانوادکی کاربر
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// تلفن کاربر
        /// </summary>
        public string? Phone { get; set; }

        public static explicit operator UserInfoViewModel(User v)
        {
            return new UserInfoViewModel
            {
                Id = v.Id,
                FirstName = v.FirstName,
                LastName = v.LastName,
                Phone = v.Phone
            };
        }
    }
}
