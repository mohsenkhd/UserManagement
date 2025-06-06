using Application.ServiceContracts.ClientAggregate;
using Application.ViewModels.ClientAggregate;
using Common.Exceptions.UserManagement;
using Context.DataBaseContext;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace UserManagement.Filters
{
    public class BackToBackAuth : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var clientService = httpContext.RequestServices.GetService<IClientService>();
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();

            string? apiKey = null;
            string? apiSecret = null;

            if (httpContext.Request.Headers.TryGetValue("X-API-KEY", out var apiKeyValues))
            {
                apiKey = apiKeyValues.FirstOrDefault() ?? throw UserManagementExceptions.ClientKeyException;
            }

            if (httpContext.Request.Headers.TryGetValue("X-API-SECRET", out var apiSecretValues))
            {
                apiSecret = apiSecretValues.FirstOrDefault() ?? throw UserManagementExceptions.ClientKeyException;
            }
            if (apiKey==null) 
            {
                throw UserManagementExceptions.ClientKeyException;
            }
            if (apiSecret == null)
            {
                throw UserManagementExceptions.ClientKeyException;
            }
            var clientModel = new GetClientCommandModel()
            {
                ApiKey = Guid.Parse(apiKey)
            };
            var client = await clientService.GetClient(clientModel);

            if (client.ApiSecret != apiSecret && (client.AuthorizedIps == null || !client.AuthorizedIps.Any(ip => ip.Equals(ipAddress))))
            {
                throw UserManagementExceptions.permissionDeniedException;
            }
        }
    }

}
