using NbaDb.Data;
using NbaDb.Models;
using NbaDbInitialiser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NbaDbInitialiser
{
    public class PlayerDbInitialiser : IPlayerDbInitialiser
    {
        private Head Headers { get; set; }
        private string[] RequiredFields { get; set; }
        private ICollection<Player> Players { get; set; }
        private NbaDbContext Context { get; set; }

        public PlayerDbInitialiser(NbaDbContext context)
        {
            SetRequired();
            this.Context = context;
        }

        private void SetRequired()
        {
            RequiredFields = new string[] 
                {
                    "PLAYER_ID", "PLAYER_NAME", "W_PCT", "MIN", "FGA", "FG3A", "FTA", "OREB", "DREB", "AST", "TOV", "STL", "BLK", "PF", "PTS", "GP"
                };
        }

        public bool DataExists()
        {
            if (Context.Player.Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Adds pre-defined headings from RequiredFields
        public void AddHeadings(string headings)
        {
            var splitHeadings = headings.Split(',');
            var headers = splitHeadings
                .Select((name, index) => new { name, index })
                .Where(h => RequiredFields.Contains(h.name))
                .Select(h => new Field(h.name, h.index))
                .ToList();
            Headers = new Head(headers);
        }

        public void AddPlayers(ICollection<Player> players)
        {
            Players = players;
        }

        public void AddPlayersToDb()
        {
            Context.AddRange(Players);
            Context.SaveChanges();
        }

        public Head GetHeaders()
        {
            return Headers;
        }
    }
}
