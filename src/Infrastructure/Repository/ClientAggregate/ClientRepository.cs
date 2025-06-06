using Application.RepositoryContracts.ClientAggregate;
using Common.Exceptions.UserManagement;
using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ClientAggregate
{

    public class ClientRepository : IClientRepository
    {
        private readonly UserManagementContext _context;
        public ClientRepository(UserManagementContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<Client> GetClient(Guid apiKey)
        {
            string apiKeyString = apiKey.ToString(); 
            var client = await _context.clients.FirstOrDefaultAsync(a => a.ApiKey == apiKeyString) ?? throw UserManagementExceptions.ClientNotFoundException;
            return client;
        }
    }
}
