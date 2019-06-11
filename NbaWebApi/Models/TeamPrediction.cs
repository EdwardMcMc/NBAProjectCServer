using NbaDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NbaWebApi.Models
{
    public class TeamPrediction
    {
        public TeamPrediction(Team T)
        {
            teamId = T.Id;
            teamName = T.Name;
            if (T.Prediction == null)
            {
                prediction = 0d;
            }
            else
            {
                prediction = (double)T.Prediction;
            }
        }
        public int teamId { get; set; }
        public string teamName { get; set; }
        public double prediction {get;set;}

    }
}
