using Application.ServiceContracts.HubLogAggregate;
using Application.ServiceContracts.UserAggregate;
using Application.ServiceContracts.UsersLoginHistory;
using Application.ViewModels.HubLog;
using Application.ViewModels.Signalr;
using Application.ViewModels.UserLoginHistory;
using Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

using System.Text.Json;
using UserManagement.Filters;

namespace UserManagement.Hub
{
    public class OnlineUserHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly IJwtService _jwtService;
        private readonly IUsersLoginHistoryService _usersLoginHistoryService;
        private readonly IHubLogService _hubLog;

        public OnlineUserHub(IConnectionMultiplexer redisConnection, IJwtService jwtService, IUsersLoginHistoryService usersLoginHistoryService, IHubLogService hubLog)
        {
            _redisConnection = redisConnection;
            _jwtService = jwtService;
            _usersLoginHistoryService = usersLoginHistoryService;
            _hubLog = hubLog;
        }
        public async Task UserOnline()
        {    
                var connectionId = Context.ConnectionId;
                var httpContext = Context.GetHttpContext();
                var token = httpContext?.Request?.Query?["access_token"];
                var decodeToken = await _jwtService.DecodeToken(token);
                //var redisDatabase = _redisConnection.GetDatabase();
                var userInfo = new UserInfo
                {
                    UserId = decodeToken.UserId,
                    FirstName = decodeToken?.FirstName ?? "",
                    LastName = decodeToken?.LastName ?? "",
                    CostumerNumber = long.TryParse(decodeToken?.CustomerNumber, out long customerNumber)
                        ? (long?)customerNumber
                        : null,
                };
            var lastStatus = await _usersLoginHistoryService.GetUserLastStatus(long.Parse( decodeToken.UserId));
            //if (lastStatus.HistoryType== UserHistoryType.Logout ) {

                var json = JsonSerializer.Serialize(userInfo);
                //await redisDatabase.StringSetAsync(decodeToken?.UserId, json);

                var AddUserLoginHistoryModel = new UserLoginHistoryAddCommandModel()
                {
                    HistoryTypeVm = UserHistoryTypeVm.Login,
                    LogDate = DateTime.Now,
                    UserId = long.TryParse(userInfo.UserId, out long userId) ? userId : 0,
                    CostumerNumber = long.TryParse(userInfo?.CostumerNumber.ToString(), out long customerNumber1)
                        ? (long?)customerNumber1
                        : null,
                };
                await _usersLoginHistoryService.AddUserLoginHistory(AddUserLoginHistoryModel);
                //await Clients.All.SendAsync("UserOnline", GetOnlineUsersFromRedis());

            //}
            //await Clients.All.SendAsync("UserOnline", GetOnlineUsersFromRedis());
        }

        public async Task UserOffline()
        {
            var connectionId = Context.ConnectionId;
            var token = Context.GetHttpContext()?.Request?.Query?["access_token"];
            var decodeToken = await _jwtService.DecodeToken(token);
            var lastStatus = await _usersLoginHistoryService.GetUserLastStatus(long.Parse(decodeToken.UserId));
            //if (lastStatus.HistoryType == UserHistoryType.Login)
            //{

                //var redisDatabase = _redisConnection.GetDatabase();
                //await redisDatabase.KeyDeleteAsync(decodeToken.UserId);
                var AddUserLoginHistoryModel = new UserLoginHistoryAddCommandModel()
                {
                    HistoryTypeVm = UserHistoryTypeVm.Logout,
                    LogDate = DateTime.Now,
                    UserId = long.Parse(decodeToken.UserId),
                    CostumerNumber = long.TryParse(decodeToken?.CustomerNumber, out long customerNumber) ? (long?)customerNumber : null,
                };
                await _usersLoginHistoryService.AddUserLoginHistory(AddUserLoginHistoryModel);
                //await Clients.All.SendAsync("UserOffline", GetOnlineUsersFromRedis());
            //}


        }

        private List<UserInfo> GetOnlineUsersFromRedis()
        {
            var redisDatabase = _redisConnection.GetDatabase();
            var server = _redisConnection.GetServer(_redisConnection.GetEndPoints().First());
            var onlineUserKeys = server.Keys(pattern: "*");
            var onlineUsers = new List<UserInfo>();
            foreach (var userKey in onlineUserKeys)
            {
                var userInfoJson = redisDatabase.StringGet(userKey);
                if (!userInfoJson.IsNullOrEmpty && userInfoJson.HasValue)
                {
                    var userInfo = JsonSerializer.Deserialize<UserInfo>(userInfoJson!);
                    onlineUsers.Add(userInfo!);
                }
            }
            return onlineUsers;
        }
    }



}

