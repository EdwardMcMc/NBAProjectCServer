using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NbaDb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NbaDb.Data
{
    public class GamesPlayedConfig : IEntityTypeConfiguration<GamesPlayed>
    {
        public void Configure(EntityTypeBuilder<GamesPlayed> builder)
        {
            builder.HasKey(gp => gp.PlayerId);
        }
    }
}
