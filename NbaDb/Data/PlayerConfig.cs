using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NbaDb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NbaDb.Data
{
    public class PlayerConfig : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedNever();
            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            builder.HasOne(p => p.GamesPlayed)
                .WithOne(gp => gp.Player)
                .HasForeignKey<GamesPlayed>(gp => gp.PlayerId);
        }
    }
}
