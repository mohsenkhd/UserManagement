namespace Application.ViewModels.AccountAggregate
{
    public class DecodeToken
    {
        /// <summary>
        /// 
        /// </summary>
        public List<string> PermissionIds { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public string GroupId { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public string UserId { get; set; } = null!;
        /// <summary>
        /// 
        /// </summary>
        public string? FirstName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? LastName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Phone { get; set; } = null!;

        public string? CustomerNumber { get; set; }
    }
}
