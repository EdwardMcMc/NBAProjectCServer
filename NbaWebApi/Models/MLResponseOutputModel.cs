using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NbaWebApi.Models
{
    public class MLResponseOutputModel
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
        [JsonProperty("Scored Labels")]
        public double ScoredLabel { get; set; }
    }
}
