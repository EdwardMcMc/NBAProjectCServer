using NbaDb.Models;
using NbaSharedModels.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NbaSharedModels.RequestModels
{
    public class TeamReqModel
    {
        [Display(Name = "teamName")]
        [Required(ErrorMessage = "The teamName field is required.")]
        [RegularExpression(@"^[\w\d\ ]+$", ErrorMessage = "At least one character was not allowed. A-Z, a-z, 0-9, space and underscore are accepted.")]
        [StringLength(50, ErrorMessage = "Max string length accepted is 50")]
        public string TeamName { get; set; }

        /// <summary>
        /// This method converts this object to a Team object. Throws an exception if TeamName is null.
        /// </summary>
        /// <returns>Team</returns>
        public Team ConvertToTeamModel()
        {
            if (String.IsNullOrEmpty(TeamName))
            {
                throw new NullTeamReqModelException("TeamReqModel.TeamName is null");
            }

            var team = new Team();
            team.Name = TeamName;

            return team;
        }
    }
}
