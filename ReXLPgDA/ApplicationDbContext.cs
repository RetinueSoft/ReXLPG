using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using static Azure.Core.HttpHeader;
using System.Security.Cryptography;
using ReXLPgDM;
using ReXL.Util;

namespace ReXLPgDA
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<UserDocument> UserDocument { get; set; }
        public DbSet<Enquiry> Enquiry { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseExceptionProcessor();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(Console.WriteLine);
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 6);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DefaultPreSetup(modelBuilder);
            UserModelCreating(modelBuilder.Entity<User>());
            UserDocumentModelCreating(modelBuilder.Entity<UserDocument>());
            EnquiryModelCreating(modelBuilder.Entity<Enquiry>());
            CommentsModelCreating(modelBuilder.Entity<Comments>());
            InitializeTriggers(modelBuilder);
            DbInitializeRecord(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        private void DefaultPreSetup(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties()
                    .Where(p => p.ClrType == typeof(DateTimeOffset)
                             || p.ClrType == typeof(DateTimeOffset?)))
                {
                    property.SetColumnType("datetimeoffset(3)");
                }
            }
        }

        private void UserModelCreating(EntityTypeBuilder<User> userBuilder)
        {
            userBuilder.HasKey(e => e.GUID);
            userBuilder.Property(e => e.GUID).ValueGeneratedOnAdd();
            userBuilder.HasIndex(o => o.Mobile).IsUnique();
        }
        private void UserDocumentModelCreating(EntityTypeBuilder<UserDocument> userDocumentBuilder)
        {
            userDocumentBuilder.HasKey(e => e.UserId);
            userDocumentBuilder.HasOne<User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
        }
        private void CommentsModelCreating(EntityTypeBuilder<Comments> commentsBuilder)
        {
            commentsBuilder.HasKey(e => e.GUID);
            commentsBuilder.Property(e => e.GUID).ValueGeneratedOnAdd();
            commentsBuilder.HasOne<User>().WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
        }
        private void EnquiryModelCreating(EntityTypeBuilder<Enquiry> enquiryBuilder)
        {
            enquiryBuilder.HasKey(e => e.GUID);
            enquiryBuilder.Property(e => e.GUID).ValueGeneratedOnAdd();
            enquiryBuilder.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Restrict);
        }
        private void InitializeTriggers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User", tb => tb.HasTrigger("UserIAuditLogTrigger"));
            modelBuilder.Entity<User>().ToTable("User", tb => tb.HasTrigger("UserUAuditLogTrigger"));
            modelBuilder.Entity<User>().ToTable("User", tb => tb.HasTrigger("UserDAuditLogTrigger"));
        }
        private void DbInitializeRecord(ModelBuilder modelBuilder)
        {
            var adminUser = new Guid("24090DF0-2507-44E1-8C5B-C1087A7C64A4");
            modelBuilder.Entity<User>().HasData(new
            {
                GUID = adminUser,
                Name = "Redmin",
                Password = "Red123!@#",
                DisplayName = "Admin",
                Mobile = "9943135008",
                RegisterDate = new DateTimeOffset(new DateTime(2025, 01, 30) , TimeSpan.Zero),
                DateOfJoining = new DateTimeOffset(new DateTime(2025, 01, 30), TimeSpan.Zero),
                About = "",
                Active = true,
                Gender = Gender.Unknown,
                Designation = UserRole.SuperAdmin,
                UserImages = "",
                LoginAllowed = true
            });
        }

    }
}
