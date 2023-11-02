using LostAndFound.Core.Entities;
using LostAndFound.Core.Entities.Common;
using LostAndFound.Core.Extensions;
using LostAndFound.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LostAndFound.Core.Enums;

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
        //Media table
        public virtual DbSet<Media> Medias { get; set; }
        public virtual DbSet<UserMedia> UserMedias { get; set; }
        public virtual DbSet<PostMedia> PostMedias { get; set; }
        public virtual DbSet<ItemMedia> ItemMedias { get; set; }
        //Violation report table
        public virtual DbSet<UserViolationReport> UserViolationReports { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        //Item table
        public virtual DbSet<Item> Items { get; set; }
        //Category table
        public virtual DbSet<Category> Categories { get; set; }
        //Category Group table
        public virtual DbSet<CategoryGroup> CategoryGroups { get; set; }
        //Location table
        public virtual DbSet<Location> Locations { get; set; }
        //Property table
        public virtual DbSet<Campus> Campuses { get; set; }
        //Post table
        public virtual DbSet<Post> Posts { get; set; }
        //Comment table
        public virtual DbSet<Comment> Comments { get; set; }
        //Post Bookmark table
        public virtual DbSet<PostBookmark> PostBookmarks { get; set; }
        //Post Flag table
        public virtual DbSet<PostFlag> PostFlags { get; set; }
        //Comment Flag table
        public virtual DbSet<CommentFlag> CommentFlags { get; set; }
        //Item Bookmark table
        public virtual DbSet<ItemBookmark> ItemBookMarks { get; set; }
        //Item Flag table
        public virtual DbSet<ItemFlag> ItemFlags { get; set; }
        //Notification table
        public virtual DbSet<Notification> Notifications { get; set; }
        //UserDevice table
        public virtual DbSet<UserDevice> UserDevices { get; set; }
        //Giveaway table
        public virtual DbSet<Giveaway> Giveaways { get; set; }
        //GiveawayParticipant table
        public virtual DbSet<GiveawayParticipant> GiveawayParticipants { get; set; }
        //ItemClaim table
        public virtual DbSet<ItemClaim> ItemClaims { get; set; }
        //Cabinet table
        public virtual DbSet<Cabinet> Cabinets { get; set; }
        //Storage table
        public virtual DbSet<Storage> Storages { get; set; }
        //Receipt table
        public virtual DbSet<Receipt> Receipts { get; set; }
        //Campus table (rename of Property table)

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserViolationReport>().HasKey(uvr => new { uvr.UserId, uvr.ReportId });
            modelBuilder.Entity<UserMedia>().HasKey(um => new { um.UserId, um.MediaId });
            modelBuilder.Entity<ItemMedia>().HasKey(im => new { im.ItemId, im.MediaId });
            modelBuilder.Entity<PostMedia>().HasKey(pm => new { pm.PostId, pm.MediaId });
            modelBuilder.Entity<PostBookmark>().HasKey(pb => new { pb.PostId, pb.UserId });
            modelBuilder.Entity<PostFlag>().HasKey(pl => new { pl.PostId, pl.UserId });
            modelBuilder.Entity<CommentFlag>().HasKey(cf => new { cf.CommentId, cf.UserId });
            modelBuilder.Entity<ItemFlag>().HasKey(itf => new { itf.ItemId, itf.UserId });
            modelBuilder.Entity<ItemBookmark>().HasKey(ib => new { ib.ItemId, ib.UserId });
            modelBuilder.Entity<GiveawayParticipant>().HasKey(gp => new { gp.GiveawayId, gp.UserId });
            modelBuilder.Entity<ItemClaim>().HasKey(ic => new { ic.ItemId, ic.UserId });
            
            modelBuilder.Entity<Role>()
                .Property(b => b.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<Location>()
                .Property(l => l.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<Core.Entities.Campus>()
                .Property(p => p.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<ItemFlag>()
                .Property(itf => itf.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<ItemBookmark>()
                .Property(ib => ib.IsActive)
                .HasDefaultValue(true);
            
            modelBuilder.Entity<Media>()
                .Property(m => m.IsActive).HasDefaultValue(true);

            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Token>()
                .HasOne(rt => rt.RefreshToken)
                .WithOne(t => t.Token)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .Property(n => n.IsRead)
                .HasDefaultValue(false);

            modelBuilder.Entity<CategoryGroup>()
                .Property(cg => cg.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Category>()
                .Property(cg => cg.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Cabinet)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CabinetId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.SaveEnumsAsString();
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            CreatedEntities();
            AuditEntities();
            SetSoftDeleteColumns();
            SetPostSoftDeleteColumns();
            SetSoftDeleteLiteColumns(); 
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
                        //entry.CurrentValues["DeletedBy"] = null;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsActive"] = false;
                        entry.CurrentValues["DeletedDate"] = DateTime.Now.ToVNTime();
                        //entry.CurrentValues["DeletedBy"] = null;
                        break;
                }
            }
        }

        private void SetSoftDeleteLiteColumns()
        {
            var entriesDeleted = ChangeTracker
                .Entries()
                .Where(e => e.Entity is ISoftDeleteLiteEntity);

            foreach (var entry in entriesDeleted)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["DeletedDate"] = null;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsActive"] = false;
                        break;
                }
            }
        }

        private void SetPostSoftDeleteColumns()
        {
            var entriesDeleted = ChangeTracker
                .Entries()
                .Where(e => e.Entity is IPostSoftDeleteEntity);

            foreach (var entry in entriesDeleted)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["DeletedDate"] = null;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["PostStatus"] = PostStatus.DELETED;
                        entry.CurrentValues["DeletedDate"] = DateTime.Now.ToVNTime();
                        break;
                }
            }
        }
    }
}
