using Application.RepositoryContracts.HubLogAggregate;
using Context.DataBaseContext;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.HubLogAggregate
{
    public class HubLogRepository : IHubLogRepository
    {
        private readonly UserManagementContext _context;
        public HubLogRepository(UserManagementContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<HubLog> AddHub(HubLog model)
        {
            _context.HubLogs.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<List<HubLog>> GetHubLogsByUserId(long userId)
        {
            return await _context.HubLogs
                .Where(log => log.UserId == userId).OrderByDescending(a=>a.Id)
                .ToListAsync();
        }
        public async Task<List<HubLog>> GetHubLastStatuses(long userId)
        {
            var res = await _context.HubLogs.Where(a => a.UserId == userId).OrderByDescending(u => u.Id).Take(2).AsNoTracking().ToListAsync();
            return res;
        }

        public int GetOpenHubIdCountWithoutClose(long userId)
        {
            var openHubIds = _context.HubLogs
                .Where(log => log.UserId == userId && log.hubType == HubType.Open)
                .Select(log => log.HubId)
                .Distinct()
                .ToList();

            var closeHubIds = _context.HubLogs
                .Where(log => log.UserId == userId && log.hubType == HubType.Close)
                .Select(log => log.HubId)
                .Distinct()
                .ToList();

            var openHubIdsWithoutClose = openHubIds.Except(closeHubIds).ToList();

            return openHubIdsWithoutClose.Count;
        }
        public async Task RemoveHubLogsByUserId(long userId)
        {
            var hubLogs = await _context.HubLogs.Where(a => a.UserId == userId).AsNoTracking().ToListAsync();
            foreach (var item in hubLogs)
            {
                if (_context.Entry(item).State == EntityState.Detached)
                {
                    _context.HubLogs.Attach(item);
                }
                _context.HubLogs.Remove(item);
            }
            await _context.SaveChangesAsync();
        }

    }
}
