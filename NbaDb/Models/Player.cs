using System;
using System.Collections.Generic;
using System.Text;

namespace NbaDb.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double W_PCT { get; set; }
        public double MIN { get; set; }
        public double FGA { get; set; }
        public double FG3A { get; set; }
        public double FTA { get; set; }
        public double OREB { get; set; }
        public double DREB { get; set; }
        public double AST { get; set; }
        public double TOV { get; set; }
        public double STL { get; set; }
        public double BLK { get; set; }
        public double PF { get; set; }
        public double PTS { get; set; }
        public ICollection<TeamPlayer> TeamPlayers { get; set; }
        public GamesPlayed GamesPlayed { get; set; }
    }
}
