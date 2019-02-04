namespace TaskManagerAPI.Persisitance
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using TaskManagerAPI.Core.Domain;

    public class TaskManagerDBContext : DbContext
    {
        public TaskManagerDBContext()
            : base("name=TaskManagerDBContext")
        {
        }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>()
                .HasKey(t => t.TaskId)
                .ToTable("Task");

            modelBuilder.Entity<Task>()
                .Property(t => t.TaskId)
                .HasColumnName("Task_ID");

            modelBuilder.Entity<Task>()
                .Property(t => t.ParentId)
                .HasColumnName("Parent_ID")
                .IsOptional();

            modelBuilder.Entity<Task>()
                .Property(t => t.TaskDetails)
                .HasColumnName("Task")
                .IsRequired();

            modelBuilder.Entity<Task>()
                .Property(t => t.StartDate)
                .HasColumnName("Start_Date");

            modelBuilder.Entity<Task>()
                .Property(t => t.EndDate)
                .HasColumnName("End_Date");

            base.OnModelCreating(modelBuilder);
        }

    }


}