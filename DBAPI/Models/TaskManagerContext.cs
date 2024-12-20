﻿using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.DB.Models;

/// <summary>
/// Контекст базы данных.
/// </summary>
public partial class TaskManagerContext : DbContext
{
  public TaskManagerContext()
  {
  }

  public TaskManagerContext(DbContextOptions<TaskManagerContext> options)
      : base(options)
  {
  }

  public virtual DbSet<Comment> Comments { get; set; }

  public virtual DbSet<Observer> Observers { get; set; }

  public virtual DbSet<Task> Tasks { get; set; }

  public virtual DbSet<User> Users { get; set; }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    using var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + ".Config.txt"));
    optionsBuilder.UseNpgsql(reader.ReadToEnd().Replace("\n", ""));
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Comment>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("Comment_pkey");

      entity.ToTable("Comment");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.CreationTime).HasColumnName("creation_time");
      entity.Property(e => e.Description).HasColumnName("description");
      entity.Property(e => e.IdCreator).HasColumnName("id_creator");
      entity.Property(e => e.IdTask).HasColumnName("id_task");

      entity.HasOne(d => d.IdCreatorNavigation).WithMany(p => p.Comments)
              .HasForeignKey(d => d.IdCreator)
              .OnDelete(DeleteBehavior.SetNull)
              .HasConstraintName("Comment_id_creator_fkey");

      entity.HasOne(d => d.IdTaskNavigation).WithMany(p => p.Comments)
              .HasForeignKey(d => d.IdTask)
              .HasConstraintName("Comment_id_task_fkey");
    });

    modelBuilder.Entity<Observer>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("Observer_pkey");

      entity.ToTable("Observer");

      entity.HasIndex(e => new { e.IdTask, e.IdUser }, "Observer_id_task_id_user_key").IsUnique();

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.IdTask).HasColumnName("id_task");
      entity.Property(e => e.IdUser).HasColumnName("id_user");

      entity.HasOne(d => d.IdTaskNavigation).WithMany(p => p.Observers)
              .HasForeignKey(d => d.IdTask)
              .HasConstraintName("Observer_id_task_fkey");

      entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Observers)
              .HasForeignKey(d => d.IdUser)
              .HasConstraintName("Observer_id_user_fkey");
    });

    modelBuilder.Entity<Task>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("Task_pkey");

      entity.ToTable("Task");

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.CreationTime).HasColumnName("creation_time");
      entity.Property(e => e.Deadline).HasColumnName("deadline");
      entity.Property(e => e.Description).HasColumnName("description");
      entity.Property(e => e.Name).HasColumnName("name");
      entity.Property(e => e.Priority).HasColumnName("priority");
      entity.Property(e => e.Responsible).HasColumnName("responsible");
      entity.Property(e => e.Status).HasColumnName("status");

      entity.HasOne(d => d.ResponsibleNavigation).WithMany(p => p.Tasks)
              .HasForeignKey(d => d.Responsible)
              .OnDelete(DeleteBehavior.SetNull)
              .HasConstraintName("Task_responsible_fkey");
    });

    modelBuilder.Entity<User>(entity =>
    {
      entity.HasKey(e => e.Id).HasName("User_pkey");

      entity.ToTable("User");

      entity.HasIndex(e => e.Email, "User_email_key").IsUnique();

      entity.HasIndex(e => e.Login, "User_login_key").IsUnique();

      entity.Property(e => e.Id).HasColumnName("id");
      entity.Property(e => e.Email).HasColumnName("email");
      entity.Property(e => e.Login).HasColumnName("login");
      entity.Property(e => e.Name).HasColumnName("name");
      entity.Property(e => e.Password).HasColumnName("password");
      entity.Property(e => e.Root).HasColumnName("root");
    });

    this.OnModelCreatingPartial(modelBuilder);
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
