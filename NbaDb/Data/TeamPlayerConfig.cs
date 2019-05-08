using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NbaDb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NbaDb.Data
{
    public class TeamPlayerConfig : IEntityTypeConfiguration<TeamPlayer>
    {
        public void Configure(EntityTypeBuilder<TeamPlayer> builder)
        {
            builder.HasKey(pt => new {pt.PlayerId,pt.TeamId });
            builder.HasOne(pt => pt.Player)
                .WithMany(p => p.TeamPlayers)
                .HasForeignKey(pt => pt.PlayerId);
            builder.HasOne(pt => pt.Team)
                .WithMany(t => t.TeamPlayers)
                .HasForeignKey(pt => pt.TeamId);
        }
    }
}
