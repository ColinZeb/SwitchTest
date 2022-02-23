using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace SwitchTest
{
    public class DateTimeDbCommandInterceptor : DbCommandInterceptor
    {
        public override InterceptionResult<DbCommand> CommandCreating(CommandCorrelatedEventData eventData, InterceptionResult<DbCommand> result)
        {
            return base.CommandCreating(eventData, result);
        }

       
        public override DbCommand CommandCreated(CommandEndEventData eventData, DbCommand result)
        {
            foreach (var a in eventData.Command.Parameters)
            {
                // not work
            }
           
            return base.CommandCreated(eventData, result);
        }
        public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
        {
            // linq , savechange all in here
            foreach (DbParameter item in command.Parameters)
            {
                if (item.Value is DateTime date)
                {
                    if (date.Kind == DateTimeKind.Local)
                    {
                        item.Value = date.ToUniversalTime();
                    }
                }
            }
            return base.ReaderExecuting(command, eventData, result);
        }
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
        {
            foreach (DbParameter item in command.Parameters)
            {
                if (item.Value is DateTime date)
                {
                    if (date.Kind == DateTimeKind.Local)
                    {
                        item.Value = date.ToUniversalTime();
                    }
                }
            }
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }
       

    }

    public class DateTimeSave: Microsoft.EntityFrameworkCore.Diagnostics.SaveChangesInterceptor//, Microsoft.EntityFrameworkCore.Diagnostics.IDbTransactionInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            return base.SavingChanges(eventData, result);
        }
    }
}
