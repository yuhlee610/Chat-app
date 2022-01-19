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
                .HasOne<MessageType>(m => m.MessageType)
                .WithMany(mt => mt.Messages)
                .HasForeignKey(m => m.SendBy);

            modelBuilder.Entity<GroupMessage>()
                .HasKey(gm => new { gm.GroupId, gm.MessageId });

            modelBuilder.Entity<MessageUser>()
                .HasKey(mu => new { mu.MessageId, mu.UserId });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageType> MessageTypes { get; set; }
        public DbSet<GroupMessage> GroupMessages { get; set; }
    }
}
