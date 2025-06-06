using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.HubLog
{
    public class GetHubLogsByUserId
    {
        public string HubId { get; set; } = null!;
        public long UserId { get; set; }
        public HubTypeVm hubType { get; set; }

        public static explicit operator GetHubLogsByUserId(Domain.Entities.HubLog v)
        {
            return new GetHubLogsByUserId
            {
                UserId = v.UserId,
                HubId = v.HubId,
                hubType =(HubTypeVm) v.hubType

            };
        }
    }
}
