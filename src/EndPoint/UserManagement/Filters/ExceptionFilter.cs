using Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;

namespace UserManagement.Filters
{
    public class ExceptionFilter : IActionFilter, IOrderedFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public ExceptionFilter(ILogger<ExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null) return;
            HandleBrokerException(context);
            HandleUnhandledException(context);
            context.ExceptionHandled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Order { get; } = 1;

        private void HandleBrokerException(ActionExecutedContext context)
        {
            if (context.Exception is not IdentityException ex) return;
            _logger.LogError("Handled exception | {Message} | {StackTrace}", ex.Message, ex.StackTrace);
            context.Result = new ObjectResult(new
            {
                ex.ClientMessage,
                ex.Code
            })
            {
                StatusCode = ex.StatusCode
            };
        }

        private void HandleUnhandledException(ActionExecutedContext context)
        {
            if (context.Exception is IdentityException) return;
            _logger.LogError("UnHandled exception | {Message} | {StackTrace}", context.Exception?.Message,
                context.Exception?.StackTrace);
            context.Result = new ObjectResult(new
            {
                ClieneMessage = "Unhandled error occurred!",
                Code = -1
            })
            {
                StatusCode = 500
            };
        }
    }
}
