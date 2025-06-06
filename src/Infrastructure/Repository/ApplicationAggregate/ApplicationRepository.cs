using Application.RepositoryContracts.ApplicationAggregate;
using Common.Exceptions.UserManagement;
using Context.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.ApplicationAggregate
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly UserManagementContext _context;

        public ApplicationRepository(UserManagementContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Domain.Entities.Application> FindByName(string name)
        {
            return await _context.Applications.SingleOrDefaultAsync(a => a.Name == name && !a.IsDeleted)??throw UserManagementExceptions.InvalidLoginException;
        }
    }
}
