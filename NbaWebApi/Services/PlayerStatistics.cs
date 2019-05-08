using NbaWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NbaWebApi.Services
{
    /// <summary>
    /// This class is used to reduce multiple player statistics into team-based statistics.
    /// Used with LINQ's Aggregate extension method.
    /// </summary>
    public class PlayerStatistics
    {
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
        public int Count { get; set; }

        /// <summary>
        /// This method is used for each enumeration.
        /// </summary>
        public PlayerStatistics Accumulate(MLPlayerModel player)
        {
            MIN += (player.Stats.MIN * player.GP);
            FGA += (player.Stats.FGA * player.GP);
            FG3A += (player.Stats.FG3A * player.GP);
            FTA += (player.Stats.FTA * player.GP);
            OREB += (player.Stats.OREB * player.GP);
            DREB += (player.Stats.DREB * player.GP);
            AST += (player.Stats.AST * player.GP);
            TOV += (player.Stats.TOV * player.GP);
            STL += (player.Stats.STL * player.GP);
            BLK += (player.Stats.BLK * player.GP);
            PF += (player.Stats.PF * player.GP);
            PTS += (player.Stats.PTS * player.GP);

            Count += 1;

            return this;
        }

        /// <summary>
        /// This method is used once, after all enumerations.
        /// </summary>
        /// <returns></returns>
        public PlayerStatistics Compute()
        {
            var games = 82;
            MIN = 48.4;
            FGA = FGA * (17/Count) / games;
            FG3A = FG3A * (17 / Count) / games;
            FTA = FTA * (17 / Count) / games;
            OREB = FTA * (17 / Count) / games;
            DREB = DREB * (17 / Count) / games;
            AST = AST * (17 / Count) / games;
            TOV = TOV * (17 / Count) / games;
            STL = STL * (17 / Count) / games;
            BLK = BLK * (17 / Count) / games;
            PF = PF * (17 / Count) / games;
            PTS = PTS * (17 / Count) / games;

            return this;
        }
    }
}
