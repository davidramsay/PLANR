using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PLANR.Models;

#nullable disable

namespace PLANR.Data
{
    public partial class TaskTrackerContext : DbContext
    {
        public TaskTrackerContext()
        {
        }

        public TaskTrackerContext(DbContextOptions<TaskTrackerContext> options)
            : base(options)
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Goal> Goals { get; set; }
        public virtual DbSet<Objective> Objectives { get; set; }
        public virtual DbSet<Record> Records { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=TaskTrackerContext");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Categoryid).HasColumnName("categoryid");

                entity.Property(e => e.CategoryAbbreviation)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("categoryAbbreviation");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("categoryName")
                    .IsFixedLength(true);

                entity.Property(e => e.UserId).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Categories)
                    .HasForeignKey(d => d.UserId)
                     .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Categories_Categories");

            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Categoryid).HasColumnName("categoryid");

                entity.Property(e => e.EventDesc)
                    .HasMaxLength(50)
                    .HasColumnName("eventDesc");

                entity.Property(e => e.EventName)
                    .HasMaxLength(50)
                    .HasColumnName("eventName");

                entity.Property(e => e.Eventid)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("eventid");

                entity.HasOne(d => d.Category)
                    .WithMany()
                    .HasForeignKey(d => d.Categoryid)
                    .HasConstraintName("FK_Events_Categories");

                entity.Property(e => e.EventStart)
                    .HasColumnType("datetime")
                    .HasColumnName("eventStart");
                entity.Property(e => e.EventEnd)
                    .HasColumnType("datetime")
                    .HasColumnName("eventEnd");
            });

            modelBuilder.Entity<Goal>(entity =>
            {
                entity.Property(e => e.Goalid).HasColumnName("goalid");

                entity.Property(e => e.Categoryid).HasColumnName("categoryid");

                entity.Property(e => e.GoalDate)
                    .HasColumnType("datetime")
                    .HasColumnName("goalDate");

                entity.Property(e => e.GoalName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("goalName")
                    .IsFixedLength(true);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Goals)
                    .HasForeignKey(d => d.Categoryid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Goals_Categories");
            });

            modelBuilder.Entity<Objective>(entity =>
            {
                entity.Property(e => e.Objectiveid).HasColumnName("objectiveid");

                entity.Property(e => e.Goalid).HasColumnName("goalid");

                entity.Property(e => e.ObjectiveName)
                   .HasMaxLength(50)
                   .HasColumnName("objectiveName");

                entity.Property(e => e.MetricName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("metricName");

                entity.Property(e => e.ObjectiveDueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("objectiveDueDate");

                entity.HasOne(d => d.Goal)
                    .WithMany(p => p.Objectives)
                    .HasForeignKey(d => d.Goalid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Objectives_Goals");
            });

            modelBuilder.Entity<Record>(entity =>
            {
                entity.Property(e => e.Recordid).HasColumnName("recordid");

                entity.Property(e => e.MetricData)
                    .HasMaxLength(10)
                    .HasColumnName("metricData")
                    .IsFixedLength(true);

                entity.Property(e => e.Objectiveid).HasColumnName("objectiveid");

                entity.Property(e => e.RecordDate)
                    .HasColumnType("datetime")
                    .HasColumnName("recordDate");

                entity.HasOne(d => d.Objective)
                    .WithMany(p => p.Records)
                    .HasForeignKey(d => d.Objectiveid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Records_Objectives");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("Task");

                entity.Property(e => e.Taskid).HasColumnName("taskid");

                entity.Property(e => e.Objectiveid).HasColumnName("objectiveid");

                entity.Property(e => e.TaskDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("taskDescription")
                    .IsFixedLength(true);

                entity.Property(e => e.TaskDueDate)
                    .HasColumnType("date")
                    .HasColumnName("taskDueDate");

                entity.Property(e => e.TaskName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("taskName")
                    .IsFixedLength(true);

                entity.Property(e => e.TaskStatus)
                    .HasColumnName("taskStatus");

                entity.HasOne(d => d.Objective)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.Objectiveid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Task_Objectives");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("userid");
                entity.Property(e => e.UserToken).HasColumnName("usertoken");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
