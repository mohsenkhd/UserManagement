using Application.ServiceContracts.ClientAggregate;
using Application.ServiceContracts.UserAggregate;
using Application.ViewModels.ClientAggregate;
using Common.Exceptions;
using Common.Exceptions.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace UserManagement.Filters
{
    public class PermissionCheckerAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly long _permissionId;

        public PermissionCheckerAttribute(long permissionId)
        {
            _permissionId = permissionId;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var config = context.HttpContext.RequestServices.GetService<IConfiguration>();
            var jwtService = context.HttpContext.RequestServices.GetService<IJwtService>();

            try
            {
                if (context.HttpContext.Request.Headers.ContainsKey("X-API-KEY") && context.HttpContext.Request.Headers.ContainsKey("X-API-SECRET"))
                {
                    var clientService = context.HttpContext.RequestServices.GetService<IClientService>();
                    var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();

                    string apiKey = context.HttpContext.Request.Headers["X-API-KEY"];
                    string apiSecret = context.HttpContext.Request.Headers["X-API-SECRET"];

                    if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
                    {
                        ReturnUnauthorizedError(context, "Api-Key Or ApiSecret NotFound");
                        return;
                    }

                    var clientModel = new GetClientCommandModel()
                    {
                        ApiKey = Guid.Parse(apiKey)
                    };

                    var client = await clientService.GetClient(clientModel);

                    if (client == null || (client.ApiSecret != apiSecret && (client.AuthorizedIps == null || !client.AuthorizedIps.Any(ip => ip.Equals(ipAddress)))))
                    {
                        ReturnForbiddenError(context, "اجازه دسترسی ندارید");
                        return;
                    }
                }
                else
                {
                    if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var tokenValues))
                    {
                        ReturnForbiddenError(context, "اجازه دسترسی ندارید");
                        return;
                    }

                    var token = tokenValues.ToString().Replace("Bearer ","");
                    if (!jwtService!.Validate(token))
                    {
                        ReturnBadRequestError(context, "Token is not valid");
                        return;
                    }

                    var userToken = await jwtService.DecodeToken(token);
                    if (!userToken.PermissionIds.Contains(_permissionId.ToString()))
                    {
                        ReturnForbiddenError(context, "اجازه دسترسی ندارید");
                        return;
                    }

                    context.HttpContext.Items["UserToken"] = userToken;
                }
            }
            catch (IdentityException ex)
            {
            
                var errorObject = new
                {
                    Code = ex.Code,
                    Message = ex.Message
                };

                context.Result = new ObjectResult(errorObject)
                {
                    StatusCode = ex.StatusCode
                };
            }
        
            catch (Exception e)
            {
                ReturnInternalServerError(context, "Unhandled error occurred!");
            }
        }

        private void ReturnUnauthorizedError(AuthorizationFilterContext context, string message)
        {
            var errorObject = new
            {
                Code = -1431,
                Message = message
            };

            context.Result = new ObjectResult(errorObject)
            {
                StatusCode = 401
            };
        }

        private void ReturnForbiddenError(AuthorizationFilterContext context, string message)
        {
            var errorObject = new
            {
                Code = -1413,
                Message = message
            };

            context.Result = new ObjectResult(errorObject)
            {
                StatusCode = 403
            };
        }

        private void ReturnBadRequestError(AuthorizationFilterContext context, string message)
        {
            var errorObject = new
            {
                Code = -1412,
                Message = message
            };

            context.Result = new ObjectResult(errorObject)
            {
                StatusCode = 400
            };
        }

        private void ReturnNotFoundError(AuthorizationFilterContext context, string message)
        {
            var errorObject = new
            {
                Code = -1430,
                Message = message
            };

            context.Result = new ObjectResult(errorObject)
            {
                StatusCode = 404
            };
        }

        private void ReturnInternalServerError(AuthorizationFilterContext context, string message)
        {
            var errorObject = new
            {
                Code = -1,
                Message = message
            };

            context.Result = new ObjectResult(errorObject)
            {
                StatusCode = 500
            };
        }
    }
}
