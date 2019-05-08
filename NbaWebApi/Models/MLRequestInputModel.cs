using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NbaWebApi.Models
{
    public class MLRequestInputModel
    {
        public string MIN { get; set; }
        public string FGA { get; set; }
        public string FG3A { get; set; }
        public string FTA { get; set; }
        public string OREB { get; set; }
        public string DREB { get; set; }
        public string AST { get; set; }
        public string TOV { get; set; }
        public string STL { get; set; }
        public string BLK { get; set; }
        public string PF { get; set; }
        public string PTS { get; set; }
    }
}
