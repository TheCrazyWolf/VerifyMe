﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VerifyMe.Storage.Context;

#nullable disable

namespace VerifyMe.Storage.Migrations
{
    [DbContext(typeof(VerifyContext))]
    partial class VerifyContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0-rc.2.24474.1");

            modelBuilder.Entity("VerifyMe.Models.DLA.App", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Apps");
                });

            modelBuilder.Entity("VerifyMe.Models.DLA.Sms", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("AppId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDelivered")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AppId");

                    b.HasIndex("UserId");

                    b.ToTable("Sms");
                });

            modelBuilder.Entity("VerifyMe.Models.DLA.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VerifyMe.Models.DLA.Sms", b =>
                {
                    b.HasOne("VerifyMe.Models.DLA.App", "App")
                        .WithMany()
                        .HasForeignKey("AppId");

                    b.HasOne("VerifyMe.Models.DLA.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("App");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}