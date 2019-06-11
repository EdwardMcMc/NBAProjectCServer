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
                new TeamPlayer{ TeamId = 1, PlayerId = 2544 },
                new TeamPlayer{ TeamId = 1, PlayerId = 201939 },
                new TeamPlayer{ TeamId = 2, PlayerId = 2544 },
                new TeamPlayer{ TeamId = 2, PlayerId = 203500 }
            };

            Context.AddRange(teamPlayers);
            Context.SaveChanges();
        }
    }
}