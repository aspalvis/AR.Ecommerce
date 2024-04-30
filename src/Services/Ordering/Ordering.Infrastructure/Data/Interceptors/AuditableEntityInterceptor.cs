using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Ordering.Domain.Abstractions;

namespace Ordering.Infrastructure.Data.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateEntities(DbContext? context)
        {
            if (context is null)
            {
                return;
            }

            foreach (var entry in context.ChangeTracker.Entries<IEntity>())
            {
                if (entry.State is EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = "Hardcoded value";
                }

                if (entry.State is EntityState.Added or EntityState.Modified || HasChangedOwnedEntities(entry))
                {
                    entry.Entity.LastModifiedBy = "Hardcoded value";
                    entry.Entity.LastModified = DateTime.UtcNow;
                }
            }
        }

        private bool HasChangedOwnedEntities(EntityEntry entry)
        {
            return entry.References.Any(x =>
                x.TargetEntry is not null &&
                x.TargetEntry.Metadata.IsOwned() &&
                x.TargetEntry.State is EntityState.Added or EntityState.Modified
            );
        }
    }
}
