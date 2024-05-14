using Ecommerce.Core.src.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ecommerce.WebAPI.src.Database
{
    public class TimeStampInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {

            var addedEntries = eventData.Context!
                        .ChangeTracker.Entries().Where(e => e.State == EntityState.Added);

            var updatedEntries = eventData.Context
            .ChangeTracker.Entries().Where(e => e.State == EntityState.Modified);

            foreach (var entry in addedEntries)
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    baseEntity.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
                    baseEntity.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
                    Console.WriteLine(baseEntity.CreatedDate.ToString());
                }
            }

            foreach (var entry in updatedEntries)
            {
                if (entry.Entity is BaseEntity baseEntity)
                {
                    baseEntity.UpdatedDate = DateOnly.FromDateTime(DateTime.Now);
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);

        }
    }
}