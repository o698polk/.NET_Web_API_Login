using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ApisEjmploApp.Models
{
    public partial class generaldbContext : DbContext
    {
        public generaldbContext()
        {
        }

        public generaldbContext(DbContextOptions<generaldbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Userdt> Userdts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {/*
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server=POLK\\SQLEXPRESS;database=generaldb;integrated security=true;");
           
                */}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Userdt>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__userdt__B9BE370F80BD3997");

                entity.ToTable("userdt");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.EmailUser)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("email_user");

                entity.Property(e => e.NameUser)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("name_user");

                entity.Property(e => e.PasswordUser)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("password_user");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
