using NbaDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NbaSharedModels.ViewModels
{
    public class TeamVm
    {
        public TeamVm()
        {

        }
        public TeamVm(Team t)
        {
            this.Id = t.Id;
            this.Name = t.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
