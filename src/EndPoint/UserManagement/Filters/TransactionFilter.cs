using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Context.DataBaseContext;

namespace UserManagement.Filters
{
    public class TransactionFilter : IActionFilter, IDisposable
    {
        private readonly UserManagementContext _context;
        private IDbContextTransaction? _transaction;

        public TransactionFilter(UserManagementContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var transactionalAttribute = context.HttpContext
                .GetEndpoint()?
                .Metadata
                .GetMetadata<UserManagement.Attributes.TransactionalAttribute>();

            if (transactionalAttribute != null)
            {
                _transaction = _context.Database.BeginTransaction();
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            if (context.Exception != null)
            {
                _transaction?.Rollback();
            }
            else
            {
                _transaction?.Commit();
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
        }
    }
}
