using NbaDb.Data;
using NbaSharedModels.ViewModels;
using NbaWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NbaWebApi.Services
{
    public class Predictor
    {
        public int MAX_PLAYERS { get; } = 15;
        private string ApiKey { get; } = @"ImWNFuOOr205x9bHV1oEEOhBJG3Vehnlyx8wjyhJgTE3ZvKQCPnyKPthwczF7Iv5wslU+W251cUN3VO+uIvxuA==";
        private string Url { get; } = @"https://ussouthcentral.services.azureml.net/workspaces/10ab5c365f0f41c69ebe26a366f7d2bd/services/082b941849aa4cf9bf87b9ff0563a425/execute?api-version=2.0&format=swagger";
        public int TeamId { get; }
        public int[] PlayerIds { get; }
        public NbaDbContext Context { get; }
        public Predictor(int teamId, int[] playerIds, NbaDbContext context)
        {
            TeamId = teamId;
            PlayerIds = playerIds;
            Context = context;
        }

        public async Task Fetch()
        {
            // Select no more than the maximum number of players
            var playerIds = PlayerIds.Take(MAX_PLAYERS).ToArray();

            // Get all the details of players from the Database
            var playerList = await Context.Player
                .Where(p => playerIds.Contains(p.Id))
                .Select(p => new PlayerVm(p))
                .AsNoTracking()
                .ToListAsync();

            // Select the games played by those players
            var gamesPlayed = await Context.GamesPlayed
                .Where(gp => playerIds.Contains(gp.PlayerId))
                .ToListAsync();

            // Join the games played to the player model, using this specific machine learning player model
            var players = playerList
                .Join(gamesPlayed, p => p.PlayerId, gp => gp.PlayerId,
                    (p, gp) => new MLPlayerModel
                    {
                        PlayerId = p.PlayerId,
                        Stats = p.Stats,
                        GP = gp.GP
                    })
                .ToList();

            // Aggregate/reduce the players stats to team stats
            var aggregatedStats = players
                .Aggregate(new PlayerStatistics(),
                    (acc, p) => acc.Accumulate(p),
                    acc => acc.Compute());

            // Create object appropriate for an Azure ML Studio request
            var predictionRequest = CreatePredictionRequestBody(aggregatedStats);

            // Fetch and return the prediction
            var prediction = await FetchPrediction(predictionRequest);

            var team = await Context.Team.Where(t => t.Id == TeamId).FirstOrDefaultAsync();
            team.Prediction = prediction.Results.output1[0].ScoredLabel;
            Context.Team.Update(team);
            
            try
            {
                await Context.SaveChangesAsync();
            }
            catch (Exception)
            {
                Console.WriteLine($"{TeamId} update failed after prediction fetched.");
            }
        }

        private MLRequestModel CreatePredictionRequestBody(PlayerStatistics aggregatedStats)
        {
            var inputs = new MLRequestInputModel[]
            {
                new MLRequestInputModel
                {
                    MIN = aggregatedStats.MIN.ToString(),
                    FGA = aggregatedStats.FGA.ToString(),
                    FG3A = aggregatedStats.FG3A.ToString(),
                    FTA = aggregatedStats.FTA.ToString(),
                    OREB = aggregatedStats.OREB.ToString(),
                    DREB = aggregatedStats.DREB.ToString(),
                    AST = aggregatedStats.AST.ToString(),
                    TOV = aggregatedStats.TOV.ToString(),
                    STL = aggregatedStats.STL.ToString(),
                    BLK = aggregatedStats.BLK.ToString(),
                    PF = aggregatedStats.PF.ToString(),
                    PTS = aggregatedStats.PTS.ToString()
                }
            };

            var input1 = new MLRequestInputListModel { input1 = inputs };
            var request = new MLRequestModel { Inputs = input1, GlobalParameters = new { } };

            return request;
        }

        private async Task<MLResponseModel> FetchPrediction(MLRequestModel data)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ApiKey);
                client.BaseAddress = new Uri(Url);
                var dataString = JsonConvert.SerializeObject(data);
                var content = new StringContent(dataString, System.Text.Encoding.UTF8, "application/json");
                var responseMessage = await client.PostAsync("", content);
                var response = await responseMessage.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<MLResponseModel>(response);
                return responseObject;
            }
        }
    }
}
