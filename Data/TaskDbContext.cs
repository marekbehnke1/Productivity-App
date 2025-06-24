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
        public DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "LearnAvalonia", "tasks.db");

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
                
                // Dont know if this will work??
                entity.Property(t => t.DueDate)
                    .HasConversion<DateTime>();
                
                // This converts the priority enum to an int
                entity.Property(t => t.TaskPriority)
                    .HasConversion<int>();
                    
            });
        }
    }
}
