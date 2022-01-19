﻿// <auto-generated />
using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Backend.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.13")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Backend.Models.Group", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("HostId")
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("HostId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Backend.Models.GroupMessage", b =>
                {
                    b.Property<string>("GroupId")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("MessageId")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("GroupId", "MessageId");

                    b.HasIndex("MessageId");

                    b.ToTable("GroupMessages");
                });

            modelBuilder.Entity("Backend.Models.GroupUser", b =>
                {
                    b.Property<string>("GroupId")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("UserId")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("GroupId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupUsers");
                });

            modelBuilder.Entity("Backend.Models.Message", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Content")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("SendBy")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("TypeId")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.HasIndex("SendBy");

                    b.HasIndex("UserId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Backend.Models.MessageType", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Type")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("Id");

                    b.ToTable("MessageTypes");
                });

            modelBuilder.Entity("Backend.Models.MessageUser", b =>
                {
                    b.Property<string>("MessageId")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("UserId")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.HasKey("MessageId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("MessageUser");
                });

            modelBuilder.Entity("Backend.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Backend.Models.Group", b =>
                {
                    b.HasOne("Backend.Models.User", "Host")
                        .WithMany("Groups")
                        .HasForeignKey("HostId");

                    b.Navigation("Host");
                });

            modelBuilder.Entity("Backend.Models.GroupMessage", b =>
                {
                    b.HasOne("Backend.Models.Group", "Group")
                        .WithMany("GroupMessages")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.Message", "Message")
                        .WithMany("GroupMessages")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Message");
                });

            modelBuilder.Entity("Backend.Models.GroupUser", b =>
                {
                    b.HasOne("Backend.Models.Group", "Group")
                        .WithMany("GroupUsers")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.User", "User")
                        .WithMany("GroupUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Models.Message", b =>
                {
                    b.HasOne("Backend.Models.MessageType", "MessageType")
                        .WithMany("Messages")
                        .HasForeignKey("SendBy");

                    b.HasOne("Backend.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("MessageType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Models.MessageUser", b =>
                {
                    b.HasOne("Backend.Models.Message", "Message")
                        .WithMany("MessageUsers")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Backend.Models.User", "User")
                        .WithMany("MessageUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Models.Group", b =>
                {
                    b.Navigation("GroupMessages");

                    b.Navigation("GroupUsers");
                });

            modelBuilder.Entity("Backend.Models.Message", b =>
                {
                    b.Navigation("GroupMessages");

                    b.Navigation("MessageUsers");
                });

            modelBuilder.Entity("Backend.Models.MessageType", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Backend.Models.User", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("GroupUsers");

                    b.Navigation("MessageUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
