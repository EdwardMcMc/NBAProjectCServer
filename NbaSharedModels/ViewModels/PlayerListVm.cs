using NbaDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NbaSharedModels.ViewModels
{
    public class PlayerListVm
    {
        public List<PlayerVm> Players { get; set; }

        public List<Player> ConvertToListOfPlayerModels()
        {
            var playerList = new List<Player>();

            foreach (PlayerVm p in Players)
            {
                var player = p.ConvertToPlayerModel();
                playerList.Add(player);
            }

            return playerList;
        }
    }
}
