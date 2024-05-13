using Ecommerce.Core.src.Entity;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ecommerce.WebAPI.src.Database
{
    public class TimeStampInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var entries = eventData.Context.ChangeTracker.Entries(); // Get all monitor entries
            var addedEntries = entries.Where(e => e.State == Microsoft.EntityFrameworkCore.EntityState.Modified);

            foreach (var entry in addedEntries)
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    baseEntity.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
                    baseEntity.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
                }
            }

            foreach (var entry in addedEntries)
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    baseEntity.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}