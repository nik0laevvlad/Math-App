using AppKurs.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppKurs.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<UserTask> UserTasks { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .Property(e => e.customUserName)
                .HasMaxLength(50);


            builder.Entity<SolvedTask>()
                .HasKey(nameof(SolvedTask.UserId), nameof(SolvedTask.TaskId));

            builder.Entity<SolvedTask>()
                .HasOne(pt => pt.Task)
                .WithMany(p => p.SolvedTasks)
                .HasForeignKey(pt => pt.TaskId);

            builder.Entity<SolvedTask>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.SolvedTasks)
                .HasForeignKey(pt => pt.UserId);
        }
    }
}
