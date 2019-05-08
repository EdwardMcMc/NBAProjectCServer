using System;
using System.Collections.Generic;
using System.Text;

namespace NbaDb.Models
{
    public class GamesPlayed
    {
        public int PlayerId { get; set; }
        public double GP { get; set; }
        public Player Player { get; set; }
    }
}
