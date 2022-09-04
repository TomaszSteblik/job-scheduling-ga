﻿// <auto-generated />
using System;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Data.Migrations
{
    [DbContext(typeof(ScheduleContext))]
    partial class ScheduleContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.8");

            modelBuilder.Entity("Data.Entities.Day", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DayOfSchedule")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Days");
                });

            modelBuilder.Entity("Data.Entities.Machine", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("varchar");

                    b.Property<int?>("PersonId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.ToTable("Machines");
                });

            modelBuilder.Entity("Data.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<string>("FirstName")
                        .HasColumnType("varchar");

                    b.Property<string>("LastName")
                        .HasColumnType("varchar");

                    b.HasKey("Id");

                    b.ToTable("People");
                });

            modelBuilder.Entity("Data.Entities.Qualification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("varchar");

                    b.HasKey("Id");

                    b.ToTable("Qualifications");
                });

            modelBuilder.Entity("DayPerson", b =>
                {
                    b.Property<int>("PeopleId")
                        .HasColumnType("integer");

                    b.Property<int>("PreferredDaysId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PeopleId", "PreferredDaysId");

                    b.HasIndex("PreferredDaysId");

                    b.ToTable("DayPerson");
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

            modelBuilder.Entity("Data.Entities.Machine", b =>
                {
                    b.HasOne("Data.Entities.Qualification", "RequiredQualification")
                        .WithMany("Machines")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Person", null)
                        .WithMany("PreferredMachines")
                        .HasForeignKey("PersonId");

                    b.Navigation("RequiredQualification");
                });

            modelBuilder.Entity("DayPerson", b =>
                {
                    b.HasOne("Data.Entities.Person", null)
                        .WithMany()
                        .HasForeignKey("PeopleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Day", null)
                        .WithMany()
                        .HasForeignKey("PreferredDaysId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PersonQualification", b =>
                {
                    b.HasOne("Data.Entities.Person", null)
                        .WithMany()
                        .HasForeignKey("PeopleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Data.Entities.Qualification", null)
                        .WithMany()
                        .HasForeignKey("QualificationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Data.Entities.Person", b =>
                {
                    b.Navigation("PreferredMachines");
                });

            modelBuilder.Entity("Data.Entities.Qualification", b =>
                {
                    b.Navigation("Machines");
                });
#pragma warning restore 612, 618
        }
    }
}
