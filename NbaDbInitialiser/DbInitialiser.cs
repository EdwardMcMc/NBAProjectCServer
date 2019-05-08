using NbaDb.Data;
using NbaDb.Models;
using NbaDbInitialiser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NbaDbInitialiser
{
    /// <summary>
    /// This class checks if the NbaDb database is empty and if so, seeds it with real, production ready Player and GamesPlayed data,
    /// as well as test data for Team and TeamPlayer.
    /// </summary>
    public class DbInitialiser
    {
        private INbaFileReader FileReader = new FileReader();
        private IPlayerDbInitialiser PlayerDbInitialiser { get; set; }
        private TeamDbInitialiser TeamInitialiser { get; set; }
        private TeamPlayerDbInitialiser TeamPlayerInitialiser { get; set; }
        private GamesPlayedDbInitialiser GamesPlayedDbInitialiser { get; set; }
        private string[] PlayerFile { get; set; }

        public DbInitialiser(NbaDbContext context)
        {
            this.PlayerDbInitialiser = new PlayerDbInitialiser(context);
            this.TeamInitialiser = new TeamDbInitialiser(context);
            this.TeamPlayerInitialiser = new TeamPlayerDbInitialiser(context);
            this.GamesPlayedDbInitialiser = new GamesPlayedDbInitialiser(context);
        }

        public void Initialise()
        {
            // Only proceed if the database is empty
            if (PlayerDbInitialiser.DataExists())
            {
                return;
            }

            ReadFile();

            // Team and TeamPlayer tables sample data is not read from a file, whereas Player and GamesPlayed do:
            // Prepare Players
            PlayerDbInitialiser.AddHeadings(PlayerFile[0]);
            var players = PlayerFile.Skip(1).Select(line => ConvertLineToPlayer(line)).ToList();
            PlayerDbInitialiser.AddPlayers(players);

            // Prepare Games Played
            GamesPlayedDbInitialiser.AddHeadings(PlayerFile[0]);
            var gamesPlayed = PlayerFile.Skip(1).Select(line => ConvertLineToGamesPlayed(line)).ToList();
            GamesPlayedDbInitialiser.Add(gamesPlayed);

            try
            {
                PlayerDbInitialiser.AddPlayersToDb();
                TeamInitialiser.AddToDb();
                TeamPlayerInitialiser.AddToDb();
                GamesPlayedDbInitialiser.AddToDb();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void ReadFile()
        {
            try
            {
                FileReader.AddFilePath("Datasets\\Players.csv");
                PlayerFile = FileReader.ReadAllLines();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Player ConvertLineToPlayer(string line)
        {
            try
            {
                var splitLine = line.Split(',');
                var fields = PlayerDbInitialiser.GetHeaders().Fields;
                var player = new Player();

                foreach (Field f in fields)
                {
                    if (f.Name == "PLAYER_ID")
                    {
                        player.Id = int.Parse(splitLine[f.Index]);
                        continue;
                    }
                    if (f.Name == "PLAYER_NAME")
                    {
                        var names = splitLine[f.Index].Split(' ');
                        names = names.Select(n => n.Replace(".", String.Empty)).ToArray();
                        player.FirstName = names[0];
                        player.LastName = string.Join(" ", names.Skip(1).Select(n => n).ToArray());
                        continue;
                    }
                    if (f.Name == "GP") continue;

                    var prop = player.GetType().GetProperty(f.Name);
                    prop.SetValue(player, double.Parse(splitLine[f.Index]));
                }

                return player;
            }
            catch (Exception)
            {
                Console.WriteLine(line.ToString());
                throw;
            }
        }

        private GamesPlayed ConvertLineToGamesPlayed(string line)
        {
            try
            {
                var splitLine = line.Split(',');
                var fields = PlayerDbInitialiser.GetHeaders().Fields;
                var gamesPlayed = new GamesPlayed();

                foreach (Field f in fields)
                {
                    if (f.Name == "PLAYER_ID")
                    {
                        gamesPlayed.PlayerId = int.Parse(splitLine[f.Index]);
                        continue;
                    }
                    if (f.Name == "GP")
                    {
                        gamesPlayed.GP = double.Parse(splitLine[f.Index]);
                        continue;
                    }
                }

                return gamesPlayed;
            }
            catch (Exception)
            {
                Console.WriteLine(line.ToString());
                throw;
            }
        }
    }
}
