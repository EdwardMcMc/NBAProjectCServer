using Microsoft.EntityFrameworkCore;
using NbaDb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NbaDb.Data
{
    public class NbaDbContext : DbContext
    {
        public DbSet<Player> Player { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<TeamPlayer> TeamPlayer { get; set; }
        public DbSet<GamesPlayed> GamesPlayed { get; set; }

        public NbaDbContext(DbContextOptions<NbaDbContext> options): base(options)
        {

        }

        public NbaDbContext()
        {

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=sb-lib-exercises.database.windows.net;Initial Catalog=NbaDb;User ID=monkiepaw;Password=ThisIsNotAGoodPassword88;Connect Timeout=60;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TeamConfig());
            modelBuilder.ApplyConfiguration(new PlayerConfig());
            modelBuilder.ApplyConfiguration(new TeamPlayerConfig());
            modelBuilder.ApplyConfiguration(new GamesPlayedConfig());
        }
    }
}
