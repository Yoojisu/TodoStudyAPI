using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TodoStudy.Database;
using TodoStudy.Attributes;
using System.Linq;

namespace TodoStudy.Database
{
    public partial class TodoDBContext : DbContext
    {
        public TodoDBContext()
        {
        }

        public TodoDBContext(DbContextOptions<TodoDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Todo> Todo { get; set; }

        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Index)
                    .HasName("PRIMARY");

                entity.ToTable("user");

                entity.Property(e => e.Index)
                    .HasColumnName("index")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Id)
                   .HasColumnName("id")
                   .HasColumnType("varchar(45)");

                entity.Property(e => e.Password)
                   .HasColumnName("password")
                   .HasColumnType("varchar(255)");


                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.CreateDate)
                   .HasColumnName("createDate")
                   .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("deleteDate")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<Todo>(entity =>
            {
                entity.HasKey(e => e.Index)
                    .HasName("PRIMARY");

                entity.ToTable("todo");

                entity.HasIndex(e => e.UserIndex)
                    .HasName("userIndex");

                entity.Property(e => e.UserIndex)
                    .HasColumnName("userIndex")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Contents)
                    .HasColumnName("contents")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.IsChecked)
                   .HasColumnName("isChecked")
                   .HasColumnType("bit(1)");

                entity.Property(e => e.CreateDate)
                   .HasColumnName("createDate")
                   .HasColumnType("datetime");

                entity.Property(e => e.DeleteDate)
                    .HasColumnName("deleteDate")
                    .HasColumnType("datetime");

               
            });

        }
        }
}
