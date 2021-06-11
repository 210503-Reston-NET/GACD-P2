﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GACDModels;

namespace GACDDL
{
    public class GACDDBContext:DbContext
    {
        public GACDDBContext(DbContextOptions options): base(options) { }
        protected GACDDBContext() { }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserStat> UserStats { get; set; }
        public DbSet<UserStatCatJoin> UserStatCatJoins { get; set; }
        public DbSet<TypeTest> TypeTests { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<CompetitionStat> CompetitionStats { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(user => user.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Category>()
                .Property(cat => cat.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<UserStat>()
                .Property(userStat => userStat.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Competition>()
                .Property(comp => comp.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<UserStatCatJoin>()
                .Property(uscj => uscj.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<CompetitionStat>()
                .HasKey(cS => new { cS.UserId, cS.CompetitionId });
            modelBuilder.Entity<TypeTest>()
                .Property(tT => tT.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
