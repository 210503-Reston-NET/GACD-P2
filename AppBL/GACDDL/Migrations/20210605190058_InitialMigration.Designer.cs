﻿// <auto-generated />
using System;
using GACDDL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GACDDL.Migrations
{
    [DbContext(typeof(GACDDBContext))]
    [Migration("20210605190058_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("GACDModels.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Name")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("GACDModels.Competition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("NumberOfParticipants")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Competitions");
                });

            modelBuilder.Entity("GACDModels.CompetitionStat", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("CompetitionId")
                        .HasColumnType("integer");

                    b.Property<double>("Accuracy")
                        .HasColumnType("double precision");

                    b.Property<double>("WPM")
                        .HasColumnType("double precision");

                    b.Property<int>("rank")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "CompetitionId");

                    b.HasIndex("CompetitionId");

                    b.ToTable("CompetitionStats");
                });

            modelBuilder.Entity("GACDModels.TypeTest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("NumberOfErrors")
                        .HasColumnType("integer");

                    b.Property<int>("NumberOfWords")
                        .HasColumnType("integer");

                    b.Property<int>("UserStatId")
                        .HasColumnType("integer");

                    b.Property<double>("WPM")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("UserStatId");

                    b.ToTable("TypeTests");
                });

            modelBuilder.Entity("GACDModels.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GACDModels.UserStat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("AverageAccuracy")
                        .HasColumnType("double precision");

                    b.Property<double>("AverageWPM")
                        .HasColumnType("double precision");

                    b.Property<int>("NumberOfTests")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("UserStats");
                });

            modelBuilder.Entity("GACDModels.UserStatCatJoin", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("UserStatId")
                        .HasColumnType("integer");

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "UserStatId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserStatId")
                        .IsUnique();

                    b.ToTable("UserStatCatJoins");
                });

            modelBuilder.Entity("GACDModels.CompetitionStat", b =>
                {
                    b.HasOne("GACDModels.Competition", "Competition")
                        .WithMany("CompetitionStats")
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GACDModels.User", "User")
                        .WithMany("CompetitionStats")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Competition");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GACDModels.TypeTest", b =>
                {
                    b.HasOne("GACDModels.UserStat", "UserStat")
                        .WithMany("TypeTests")
                        .HasForeignKey("UserStatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserStat");
                });

            modelBuilder.Entity("GACDModels.UserStatCatJoin", b =>
                {
                    b.HasOne("GACDModels.Category", null)
                        .WithMany("UserStatCatJoins")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GACDModels.User", "User")
                        .WithMany("UserStatCatJoins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GACDModels.UserStat", "UserStat")
                        .WithOne("UserStatCatJoin")
                        .HasForeignKey("GACDModels.UserStatCatJoin", "UserStatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("UserStat");
                });

            modelBuilder.Entity("GACDModels.Category", b =>
                {
                    b.Navigation("UserStatCatJoins");
                });

            modelBuilder.Entity("GACDModels.Competition", b =>
                {
                    b.Navigation("CompetitionStats");
                });

            modelBuilder.Entity("GACDModels.User", b =>
                {
                    b.Navigation("CompetitionStats");

                    b.Navigation("UserStatCatJoins");
                });

            modelBuilder.Entity("GACDModels.UserStat", b =>
                {
                    b.Navigation("TypeTests");

                    b.Navigation("UserStatCatJoin");
                });
#pragma warning restore 612, 618
        }
    }
}
