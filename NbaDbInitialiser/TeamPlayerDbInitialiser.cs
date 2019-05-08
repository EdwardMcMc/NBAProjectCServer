using NbaDb.Data;
using NbaDb.Models;

namespace NbaDbInitialiser
{
    public class TeamPlayerDbInitialiser
    {
        private NbaDbContext Context { get; set; }

        public TeamPlayerDbInitialiser(NbaDbContext context)
        {
            this.Context = context;
        }

        public void AddToDb()
        {
            var teamPlayers = new TeamPlayer[]
            {
                new TeamPlayer{ TeamId = 9633324, PlayerId = 2544 },
                new TeamPlayer{ TeamId = 9633324, PlayerId = 201939 },
                new TeamPlayer{ TeamId = 42, PlayerId = 2544 },
                new TeamPlayer{ TeamId = 42, PlayerId = 203500 }
            };

            Context.AddRange(teamPlayers);
            Context.SaveChanges();
        }
    }
}