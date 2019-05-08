using NbaDb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NbaSharedModels.ViewModels
{
    public class TeamPlayerVm
    {
        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "teamId")]
        public int TeamId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name = "teamId")]
        public int PlayerId { get; set; }

        [RegularExpression(@"^[\w\ \']+$", ErrorMessage = "At least one character was not allowed. A-Z, a-z, space, apostrophe and underscore are accepted.")]
        [StringLength(100)]
        [Display(Name = "firstName")]
        public string FirstName { get; set; }

        [RegularExpression(@"^[\w\ \']+$", ErrorMessage = "At least one character was not allowed. A-Z, a-z, space, apostrophe and underscore are accepted.")]
        [StringLength(100)]
        [Display(Name = "lastName")]
        public string LastName { get; set; }

        public StatsVm Stats { get; set; }

        public TeamPlayerVm()
        {

        }

        public TeamPlayerVm(TeamPlayer tp)
        {
            TeamId = tp.TeamId;
            PlayerId = tp.PlayerId;
            FirstName = tp.Player.FirstName;
            LastName = tp.Player.LastName;
            Stats = CreateStats(tp.Player);
            
        }

        private StatsVm CreateStats(Player p)
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
    }
}
