using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LearnAvalonia.Models;
using System.IO;

namespace LearnAvalonia.Data
{

    public class TaskDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Setting the local database path
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LearnAvalonia", "tasks.db");

            // Check if directory exists, if not, create
            var directory = Path.GetDirectoryName(dbPath);
            if (!Directory.Exists(directory) && directory != null)
            {
                Directory.CreateDirectory(directory);
            }

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Defines how TaskItem will map to the db tables
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title)
                    .IsRequired()
                    .HasMaxLength(200);
                
                entity.Property(t => t.Description)
                    .HasMaxLength(1000);

                entity.Property(t => t.DueDate);
                
                // This converts the priority enum to an int
                entity.Property(t => t.TaskPriority)
                    .HasConversion<int>();
                    
            });

            // Defining the Projects table
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

                entity.Property(t => t.Description) 
                .HasMaxLength(1000);

                entity.Property(t => t.DateCreated) 
                .IsRequired();
                   
            });


            // This defines the relationship between taskitem and project
            modelBuilder.Entity<TaskItem>()         
                .HasOne<Project>()                      // Task item only belongs to one project
                .WithMany()                             // Project can have many taskitems
                .HasForeignKey(t => t.ProjectId)        // Taskitem foreign key is projectid
                .OnDelete(DeleteBehavior.SetNull);      // If project deleted - set taskitem projectid to null
        }
    }
}
