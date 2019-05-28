using System;
using System.Collections.Generic;
using System.Text;

namespace NbaDb.Models
{
   public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Prediction { get; set; }
        public int? HashCode { get; set; }

        public ICollection<TeamPlayer> TeamPlayers { get; set; }
    }
}
