using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RepositoryContracts.ApplicationAggregate
{
    public interface IApplicationRepository
    {
        public Task<Domain.Entities.Application> FindByName(string name);
        //public Task<Domain.Entities.Application> Login(string name, string password);
    }
}
