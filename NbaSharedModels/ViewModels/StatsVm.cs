using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NbaSharedModels.ViewModels
{
    public class StatsVm
    {
        [JsonProperty("W_PCT")]
        public double W_PCT { get; set; }
        [JsonProperty("MIN")]
        public double MIN { get; set; }
        [JsonProperty("FGA")]
        public double FGA { get; set; }
        [JsonProperty("FG3A")]
        public double FG3A { get; set; }
        [JsonProperty("FTA")]
        public double FTA { get; set; }
        [JsonProperty("OREB")]
        public double OREB { get; set; }
        [JsonProperty("DREB")]
        public double DREB { get; set; }
        [JsonProperty("AST")]
        public double AST { get; set; }
        [JsonProperty("TOV")]
        public double TOV { get; set; }
        [JsonProperty("STL")]
        public double STL { get; set; }
        [JsonProperty("BLK")]
        public double BLK { get; set; }
        [JsonProperty("PF")]
        public double PF { get; set; }
        [JsonProperty("PTS")]
        public double PTS { get; set; }
    }
}
