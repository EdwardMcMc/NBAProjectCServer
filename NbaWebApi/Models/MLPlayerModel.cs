using NbaSharedModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NbaWebApi.Models
{
    public class MLPlayerModel : PlayerVm
    {
        public double GP { get; set; }
    }
}
