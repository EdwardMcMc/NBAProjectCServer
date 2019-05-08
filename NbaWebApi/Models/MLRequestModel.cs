using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NbaWebApi.Models
{
    public class MLRequestModel
    {
        public MLRequestInputListModel Inputs { get; set; }
        public object GlobalParameters { get; set; }
    }
}
