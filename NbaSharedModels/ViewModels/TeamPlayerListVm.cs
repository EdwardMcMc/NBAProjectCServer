using NbaDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NbaSharedModels.ViewModels
{
    public class TeamPlayerListVm
    {
        public List<TeamPlayerVm> Players { get; set; }

        public TeamPlayerListVm()
        {

        }

        public TeamPlayerListVm(List<TeamPlayer> teamPlayers)
        {
            Players = teamPlayers
                .Select(tp => new TeamPlayerVm(tp))
                .ToList();
        }

        public List<TeamPlayer> ConvertToListOfTeamPlayerModels()
        {
            var teamPlayers = Players
                .Select(tp => 
                {
                    var teamPlayer = new TeamPlayer();
                    teamPlayer.PlayerId = tp.PlayerId;
                    teamPlayer.TeamId = tp.TeamId;
                    return teamPlayer;
                })
                .ToList();

            return teamPlayers;
        }
    }
}
