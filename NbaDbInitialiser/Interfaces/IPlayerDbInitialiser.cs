using NbaDb.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NbaDbInitialiser.Interfaces
{
    public interface IPlayerDbInitialiser
    {
        bool DataExists();
        void AddHeadings(string headings);
        void AddPlayers(ICollection<Player> players);
        void AddPlayersToDb();
        Head GetHeaders();
    }
}
