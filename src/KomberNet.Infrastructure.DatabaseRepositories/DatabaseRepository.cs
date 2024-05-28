// Copyright Contributors to the KomberNet project.
// This file is licensed to you under the Apache License, Version 2.0.
// See the LICENSE and NOTICE files in the project root for full license information.

namespace KomberNet.Infrastructure.DatabaseRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using KomberNet.Contracts;
    using KomberNet.Exceptions;
    using KomberNet.Models.Contracts;
    using Microsoft.EntityFrameworkCore;

    public abstract class DatabaseRepository<TDbContext> : IDatabaseRepository<TDbContext>
        where TDbContext : DbContext
    {
        private readonly TDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ICurrentUserService currentUserService;

        public DatabaseRepository(
            TDbContext dbContext,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.currentUserService = currentUserService;
        }

        public TDatabaseEntity ApplyChanges<TDatabaseEntity, TEntity>(TEntity entity)
            where TDatabaseEntity : class, IDatabaseEntity
            where TEntity : class, IEntity
        {
            var databaseEntity = this.mapper.Map<TDatabaseEntity>(entity);
            this.dbContext.ChangeTracker.TrackGraph(databaseEntity, x =>
            {
                if (x.Entry.State == EntityState.Detached
                    && x.Entry.Entity is IHasDataState trackedEntity)
                {
                    switch (trackedEntity.DataState)
                    {
                        case DataState.Unchanged:
                            x.Entry.State = EntityState.Unchanged;
                            break;
                        case DataState.Inserted:
                            x.Entry.State = EntityState.Added;
                            break;
                        case DataState.Updated:
                            x.Entry.State = EntityState.Modified;
                            break;
                        case DataState.Deleted:
                            x.Entry.State = EntityState.Deleted;
                            break;
                        default:
                            break;
                    }

                    if (x.Entry.State != EntityState.Unchanged)
                    {
                        this.SetAuditLogFields(x.Entry.Entity);
                    }
                }
            });

            return databaseEntity;
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await this.dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException concurrencyException)
            {
                throw new KomberNetException(ExceptionCode.ConcurrencyException, innerException: concurrencyException);
            }
        }

        public async Task<IList<TDestination>> GetAllAsync<TDatabaseEntity, TDestination>(CancellationToken cancellationToken = default)
            where TDatabaseEntity : class, IDatabaseEntity
            where TDestination : class
        {
            return await this.mapper.ProjectTo<TDestination>(this.dbContext.Set<TDatabaseEntity>().AsQueryable()).ToListAsync(cancellationToken);
        }

        public async Task<IList<TDestination>> GetByConditionAsync<TDatabaseEntity, TDestination>(Func<IQueryable<TDatabaseEntity>, IQueryable<TDatabaseEntity>> queryable, CancellationToken cancellationToken = default)
            where TDatabaseEntity : class, IDatabaseEntity
            where TDestination : class
        {
            return await this.mapper.ProjectTo<TDestination>(queryable(this.dbContext.Set<TDatabaseEntity>().AsQueryable())).ToListAsync(cancellationToken);
        }

        private void SetAuditLogFields(object entity)
        {
            if (entity is IHasDataState dataStateEntity
                && entity is IHasAuditLog auditLogEntity)
            {
                var userId = this.currentUserService.UserId;
                var userName = this.currentUserService.FullName;
                var now = DateTime.Now;

                if (dataStateEntity.DataState == DataState.Inserted)
                {
                    auditLogEntity.CreatedAt = now;
                    auditLogEntity.CreatedByUserId = userId;
                    auditLogEntity.CreatedByUserName = userName;
                }

                auditLogEntity.UpdatedAt = now;
                auditLogEntity.UpdatedByUserId = userId;
                auditLogEntity.UpdatedByUserName = userName;
            }
        }
    }
}
