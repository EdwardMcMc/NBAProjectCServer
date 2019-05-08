using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NbaDb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NbaDb.Data
{
    public class TeamConfig: IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.HasKey(t => t.Id);
            builder.HasIndex(t => t.Name).IsUnique();
            builder.Property(t => t.Id).ValueGeneratedOnAdd().UseSqlServerIdentityColumn();
            builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
        }
    }
}
