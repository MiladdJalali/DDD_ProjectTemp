using Mediator;
using Microsoft.EntityFrameworkCore;
using ProjectTemp.Application;
using ProjectTemp.Domain;
using ProjectTemp.Infrastructure.Notifications;

namespace ProjectTemp.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WriteDbContext dbContext;

        private readonly ISystemDateTime systemDateTime;

        private readonly IMediator mediator;

        private readonly IUserDescriptor userDescriptor;

        public UnitOfWork(
            WriteDbContext dbContext,
            IMediator mediator,
            IUserDescriptor userDescriptor,
            ISystemDateTime systemDateTime)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
            this.userDescriptor = userDescriptor;
            this.systemDateTime = systemDateTime;
        }

        public Task BeginTransaction(CancellationToken cancellationToken = default)
        {
            return dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransaction(CancellationToken cancellationToken = default)
        {
            await PublishDomainEvents(cancellationToken).ConfigureAwait(false);
            await dbContext.Database.CommitTransactionAsync(cancellationToken).ConfigureAwait(false);
            await mediator.Publish(new EntitiesPersistedNotification(), cancellationToken).ConfigureAwait(false);
        }

        public Task RollbackTransaction(CancellationToken cancellationToken = default)
        {
            return dbContext.Database.RollbackTransactionAsync(cancellationToken);
        }

        private async Task PublishDomainEvents(CancellationToken cancellationToken)
        {
            while (true)
            {
                var entries = dbContext.ChangeTracker.Entries<Entity>().ToList();

                if (entries.Any(i => i.State == EntityState.Deleted && !i.Entity.CanBeDeleted()))
                    throw new InvalidOperationException();

                SetAuditingProperties();

                var domainEvents = entries
                    .Where(i => i.Entity.DomainEvents.Any())
                    .SelectMany(i => i.Entity.DomainEvents)
                    .ToArray();

                entries.ForEach(i => i.Entity.ClearEvents());
                await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                if (!domainEvents.Any())
                    break;

                foreach (var domainEvent in domainEvents)
                    await mediator.Publish(domainEvent, cancellationToken).ConfigureAwait(false);
            }

            await mediator.Publish(new DomainEventsPublishedNotification(), cancellationToken).ConfigureAwait(false);
            await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }

        private void SetAuditingProperties()
        {
            var entries = dbContext.ChangeTracker
                .Entries<Entity>()
                .Where(i => i.State == EntityState.Added || i.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatorId").CurrentValue = userDescriptor.GetId();
                    entry.Property("Created").CurrentValue = systemDateTime.UtcNow;
                }

                if (entry.State != EntityState.Modified)
                    continue;

                entry.Property("UpdaterId").CurrentValue = userDescriptor.GetId();
                entry.Property("Updated").CurrentValue = systemDateTime.UtcNow;
            }
        }
    }
}