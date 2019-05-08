using System;
using System.Collections.Generic;
using System.Text;

namespace NbaDb.Models
{
    public class TeamPlayer
    {
        public int PlayerId { get; set; }
        public int TeamId { get; set; }
        public Player Player { get; set; }
        public Team Team { get; set; }
    }
}
