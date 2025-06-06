using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepositoryContracts.ClientAggregate
{
    public interface IClientRepository
    {
        Task<Client> GetClient(Guid apiKey);
    }
}
