using Application.RepositoryContracts.ClientAggregate;
using Application.ServiceContracts.ClientAggregate;
using Application.ViewModels.ClientAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.ClientAggregate
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        public ClientService(IClientRepository clientRepository)
        {
                _clientRepository = clientRepository;
        }
        public async Task<GetClientViewModel> GetClient(GetClientCommandModel model)
        {
            var client = await _clientRepository.GetClient(model.ApiKey);
            return new GetClientViewModel()
            {
                ApiKey = client.ApiKey,
                ApiSecret = client.ApiKey,
                AuthorizedIps = client.AuthorizedIps.Split(","),
                IsActive = client.IsActive,
                Name = client.Name,
            };
        }
    }
}
