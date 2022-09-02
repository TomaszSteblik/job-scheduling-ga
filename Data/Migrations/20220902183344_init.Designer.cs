﻿// <auto-generated />
using System;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(ScheduleContext))]
    [Migration("20220902183344_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("Data.Models.Machine", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<int?>("PersonId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("Machines");
                });

            modelBuilder.Entity("Data.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("Data.Models.Qualification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar");

                    b.HasKey("Id");

                    b.ToTable("Qualifications");
                });

            modelBuilder.Entity("PersonQualification", b =>
                {
                    b.Property<int>("PeopleId")
                        .HasColumnType("integer");

                    b.Property<int>("QualificationsId")
                        .HasColumnType("integer");

                    b.HasKey("PeopleId", "QualificationsId");

                    b.HasIndex("QualificationsId");

                    b.ToTable("PersonQualification");
                });

            modelBuilder.Entity("Data.Models.Machine", b =>
                {
                    b.HasOne("Data.Models.Qualification", "RequiredQualification")
                        .WithMany("Machines")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Models.Person", null)
                        .WithMany("PreferredMachines")
                        .HasForeignKey("PersonId");

                    b.Navigation("RequiredQualification");
                });

            modelBuilder.Entity("PersonQualification", b =>
                {
                    b.HasOne("Data.Models.Person", null)
                        .WithMany()
                        .HasForeignKey("PeopleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Models.Qualification", null)
                        .WithMany()
                        .HasForeignKey("QualificationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Models.Person", b =>
                {
                    b.Navigation("PreferredMachines");
                });

            modelBuilder.Entity("Data.Models.Qualification", b =>
                {
                    b.Navigation("Machines");
                });
#pragma warning restore 612, 618
        }
    }
}
