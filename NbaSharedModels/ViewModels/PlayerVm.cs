using NbaDb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NbaSharedModels.ViewModels
{
    public class PlayerVm
    {
        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "playerId")]
        public int PlayerId { get; set; }

        [Required]
        [RegularExpression(@"^[\w\ \']+$", ErrorMessage = "At least one character was not allowed. A-Z, a-z, space, apostrophe and underscore are accepted.")]
        [StringLength(100)]
        [Display(Name = "firstName")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[\w\ \']+$", ErrorMessage = "At least one character was not allowed. A-Z, a-z, space, apostrophe and underscore are accepted.")]
        [StringLength(100)]
        [Display(Name = "lastName")]
        public string LastName { get; set; }

        public StatsVm Stats { get; set; }

        public PlayerVm()
        {

        }

        public PlayerVm(Player p)
        {
            PlayerId = p.Id;
            FirstName = p.FirstName;
            LastName = p.LastName;
            Stats = createStatsList(p);
        }

        private StatsVm createStatsList(Player p)
        {
            var stats = new StatsVm();
            stats.AST = p.AST;
            stats.BLK = p.BLK;
            stats.DREB = p.DREB;
            stats.FG3A = p.FG3A;
            stats.FGA = p.FGA;
            stats.FTA = p.FTA;
            stats.MIN = p.MIN;
            stats.OREB = p.OREB;
            stats.PF = p.PF;
            stats.PTS = p.PTS;
            stats.STL = p.STL;
            stats.TOV = p.TOV;
            stats.W_PCT = p.W_PCT;

            return stats;
        }

        public Player ConvertToPlayerModel()
        {
            try
            {
                var player = new Player();
                player.Id = PlayerId;
                player.FirstName = FirstName;
                player.LastName = LastName;
                player.AST = Stats.AST;
                player.BLK = Stats.BLK;
                player.DREB = Stats.DREB;
                player.FG3A = Stats.FG3A;
                player.FGA = Stats.FGA;
                player.FTA = Stats.FTA;
                player.MIN = Stats.MIN;
                player.OREB = Stats.OREB;
                player.PF = Stats.PF;
                player.PTS = Stats.PTS;
                player.STL = Stats.STL;
                player.TOV = Stats.TOV;
                player.W_PCT = Stats.W_PCT;

                return player;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException($"{ex.Message}");
            }
        }
    }
}
