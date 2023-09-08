﻿// <auto-generated />
using System;
using LostAndFound.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LostAndFound.API.Migrations
{
    [DbContext(typeof(LostAndFoundDbContext))]
    partial class LostAndFoundDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LostAndFound.Core.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryGroupId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.CategoryGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("CategoryGroups");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<Guid?>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DeletedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FoundDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FoundUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ItemStatus")
                        .HasColumnType("int");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("FoundUserId");

                    b.HasIndex("LocationId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Floor")
                        .HasColumnType("int");

                    b.Property<bool?>("IsActive")
                        .IsRequired()
                        .HasColumnType("bit");

                    b.Property<string>("LocationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PropertyId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PropertyId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.Media", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Medias");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.Property", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("IsActive")
                        .IsRequired()
                        .HasColumnType("bit");

                    b.Property<string>("PropertyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TokenId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("TokenId")
                        .IsUnique();

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.Property<bool?>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.Token", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.UserMedia", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("MediaId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "MediaId");

                    b.HasIndex("MediaId");

                    b.ToTable("UserMedias");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.UserViolationReport", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ReportId")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ReportId");

                    b.HasIndex("ReportId");

                    b.ToTable("UserViolationReports");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.ViolationReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Category")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ViolationReports");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.Item", b =>
                {
                    b.HasOne("LostAndFound.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("FoundUserId");

                    b.HasOne("LostAndFound.Core.Entities.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.Location", b =>
                {
                    b.HasOne("LostAndFound.Core.Entities.Property", "Property")
                        .WithMany()
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Property");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.RefreshToken", b =>
                {
                    b.HasOne("LostAndFound.Core.Entities.Token", "Token")
                        .WithOne("RefreshToken")
                        .HasForeignKey("LostAndFound.Core.Entities.RefreshToken", "TokenId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Token");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.Token", b =>
                {
                    b.HasOne("LostAndFound.Core.Entities.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.User", b =>
                {
                    b.HasOne("LostAndFound.Core.Entities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.UserMedia", b =>
                {
                    b.HasOne("LostAndFound.Core.Entities.Media", "Media")
                        .WithMany("UserMedias")
                        .HasForeignKey("MediaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LostAndFound.Core.Entities.User", "User")
                        .WithMany("UserMedias")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Media");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.UserViolationReport", b =>
                {
                    b.HasOne("LostAndFound.Core.Entities.ViolationReport", "Report")
                        .WithMany("UserViolationReports")
                        .HasForeignKey("ReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LostAndFound.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Report");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.Media", b =>
                {
                    b.Navigation("UserMedias");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.Token", b =>
                {
                    b.Navigation("RefreshToken");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.User", b =>
                {
                    b.Navigation("Tokens");

                    b.Navigation("UserMedias");
                });

            modelBuilder.Entity("LostAndFound.Core.Entities.ViolationReport", b =>
                {
                    b.Navigation("UserViolationReports");
                });
#pragma warning restore 612, 618
        }
    }
}
