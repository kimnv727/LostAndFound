﻿using LostAndFound.Core.Entities;
using LostAndFound.Core.Entities.Common;
using LostAndFound.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LostAndFound.Infrastructure.Data
{
    public class LostAndFoundDbContext : DbContext
    {
        public LostAndFoundDbContext(DbContextOptions<LostAndFoundDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("LostAndFoundDB"));
        }

        //User table
        public virtual DbSet<User> Users { get; set; }
        //Role table
        public virtual DbSet<Role> Roles { get; set; }
        //Token table
        public virtual DbSet<Token> Tokens { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CreatedEntities();
            AuditEntities();
            SetSoftDeleteColumns();
            //await DispatchEvents<DomainEvent>();
            var result = await base.SaveChangesAsync(cancellationToken);
            //_ = DispatchEvents<IntegrationEvent>();

            return result;
        }

        private void CreatedEntities()
        {
            foreach (var entry in ChangeTracker.Entries<ICreatedEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedDate = DateTime.Now.ToVNTime();
                }
            }
        }

        private void AuditEntities()
        {
            foreach (var entry in ChangeTracker.Entries<IAuditedEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now.ToVNTime();
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = DateTime.Now.ToVNTime();
                        break;
                }
            }
        }

        private void SetSoftDeleteColumns()
        {
            var entriesDeleted = ChangeTracker
                .Entries()
                .Where(e => e.Entity is ISoftDeleteEntity);

            foreach (var entry in entriesDeleted)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["DeletedDate"] = null;
                        entry.CurrentValues["DeletedBy"] = null;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsActive"] = false;
                        entry.CurrentValues["DeletedDate"] = DateTime.Now.ToVNTime();
                        entry.CurrentValues["DeletedBy"] = null;
                        break;
                }
            }
        }
    }
}
