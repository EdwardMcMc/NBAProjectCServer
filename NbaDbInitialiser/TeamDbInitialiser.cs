using Microsoft.EntityFrameworkCore;
using NbaDb.Data;
using NbaDb.Models;

namespace NbaDbInitialiser
{
    public class TeamDbInitialiser
    {
        private NbaDbContext Context { get; set; }
        public TeamDbInitialiser(NbaDbContext context)
        {
            this.Context = context;
        }

        public void AddToDb()
        {
            var teams = new Team[]
            {
                new Team{ Id = 9633324, Name = "Team Richo" },
                new Team{ Id = 42, Name = "TestTeam" }
            };

            using (var transaction = Context.Database.BeginTransaction())
            {
                Context.AddRange(teams);
                Context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Team ON");
                Context.SaveChanges();
                Context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Team OFF");
                transaction.Commit();
            }
        }
    }
}