using NbaDb.Data;
using NbaDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NbaDbInitialiser
{
    public class GamesPlayedDbInitialiser
    {
        private Head Headers { get; set; }
        private string[] RequiredFields { get; set; }
        private ICollection<GamesPlayed> GamesPlayed { get; set; }
        private NbaDbContext Context { get; set; }

        public GamesPlayedDbInitialiser(NbaDbContext context)
        {
            SetRequired();
            this.Context = context;
        }

        private void SetRequired()
        {
            RequiredFields = new string[]
                {
                    "PLAYER_ID", "GP"
                };
        }

        public bool DataExists()
        {
            if (Context.GamesPlayed.Any())
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

        public void Add(ICollection<GamesPlayed> gamesPlayed)
        {
            GamesPlayed = gamesPlayed;
        }

        public void AddToDb()
        {
            Context.AddRange(GamesPlayed);
            Context.SaveChanges();
        }

        public Head GetHeaders()
        {
            return Headers;
        }
    }
}
