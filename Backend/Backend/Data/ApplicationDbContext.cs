using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>()
                .HasOne<User>(g => g.Host)
                .WithMany(u => u.Groups)
                .HasForeignKey(g => g.HostId);

            modelBuilder.Entity<GroupUser>()
                .HasKey(gu => new { gu.GroupId, gu.UserId });

            modelBuilder.Entity<Message>()
                .HasOne<User>(g => g.ToUser)
                .WithMany(u => u.MessageToUser)
                .HasForeignKey(g => g.ToUserId);

            modelBuilder.Entity<Message>()
                .HasOne<Group>(g => g.ToGroup)
                .WithMany(u => u.MessageToGroup)
                .HasForeignKey(g => g.ToGroupId);

            modelBuilder.Entity<Message>()
                .HasOne<User>(g => g.SendByUser)
                .WithMany(u => u.MessagesOfUser)
                .HasForeignKey(g => g.SendByUserId);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
