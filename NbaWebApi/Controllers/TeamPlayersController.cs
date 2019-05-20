using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NbaDb.Data;
using NbaDb.Models;
using NbaWebApi.ExceptionHandlers;
using NbaSharedModels.ViewModels;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;

namespace NbaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class TeamPlayersController : ControllerBase
    {
        private readonly NbaDbContext _context;
        private readonly int MAX_PLAYERS = 15;

        public TeamPlayersController(NbaDbContext context)
        {
            _context = context;
        }

        // GET: api/TeamPlayers
        [HttpGet]
        public async Task<TeamPlayerListVm> GetTeamPlayers()
        {
            //return _context.PlayerTeam;
            var response = new TeamPlayerListVm();

            response.Players = await _context.TeamPlayer
                .Include(tp => tp.Player)
                .Select(tp => new TeamPlayerVm(tp))
                .ToListAsync();
            return response;
        }
        // .Select(tp => new TeamPlayerVm((p => new PlayerVm(p)), tp)

        // GET: api/TeamPlayers/5
        [HttpGet("{teamId}")]
        public async Task<IActionResult> GetTeamPlayer([FromRoute] int teamId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teamPlayers = await _context
                .TeamPlayer
                .Include(tp => tp.Player)
                .Where(tp => tp.TeamId == teamId)
                .ToListAsync();
            if (teamPlayers.Count()==0)
            {
                return NotFound($"No players exists with teamId: { teamId}");
            }

            TeamPlayerListVm TPL = new TeamPlayerListVm(teamPlayers);
            return Ok(TPL);
        }

        // PUT: api/TeamPlayers/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTeamPlayer([FromRoute] int id, [FromBody] TeamPlayer teamPlayer)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != teamPlayer.PlayerId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(teamPlayer).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TeamPlayerExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/TeamPlayers
        [HttpPost("{teamId}")]
        public async Task<IActionResult> PostTeamPlayer([FromRoute] int teamId, [FromBody] TeamPlayerListVm teamPlayerList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate that adding these requested players to a team would not pass the MAX_PLAYER limit.
            var teamPlayerListCount = teamPlayerList.Players.Count();
            var dbCount = _context.TeamPlayer.AsNoTracking().Where(tp => tp.TeamId == teamId).Count();
            if (dbCount + teamPlayerListCount  > MAX_PLAYERS)
            {
                return BadRequest($"Adding {teamPlayerListCount} players from request would exceed the max player limit of {MAX_PLAYERS}.");
            }

            var teamPlayers = teamPlayerList.ConvertToListOfTeamPlayerModels();

            foreach (TeamPlayer tp in teamPlayers)
            {
                TryValidateModel(tp);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }

            // Validation for duplicate PlayerIds
            var distinctPlayers = teamPlayers.Select(tp => tp.PlayerId).Distinct().ToArray();
            if (distinctPlayers.Count() != teamPlayers.Count())
            {
                return BadRequest("At least 1 duplicate playerId in request.");
            }
            // Validation for multiple teamIds
            var distinctTeams = teamPlayers.Where(tp => tp.TeamId != teamId).Select(tp => tp.TeamId).ToArray();
            if (distinctTeams.Count() > 0)
            {
                return BadRequest("More than 1 teamId in request.");
            }

            _context.TeamPlayer.AddRange(teamPlayers);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when ((ex.InnerException as SqlException)?.Number == 2627)
            {
                var filteredPlayerIds = await _context.TeamPlayer.AsNoTracking().Where(tp => tp.TeamId == teamId).Select(tp => tp.PlayerId).ToArrayAsync();
                var filteredRequestPlayerIds = teamPlayers.Select(tp => tp.PlayerId).ToArray();
                var existing = filteredPlayerIds.Where(f => filteredRequestPlayerIds.Contains(f)).Select(f => f).ToArray();
                var message = SqlExceptionHandlers.DuplicateCompositePrimaryKeyException(ex, existing);
                return BadRequest(message);
            }
            catch (DbUpdateException ex) when ((ex.InnerException as SqlException)?.Number == 547)
            {
                return BadRequest($"Team Id or Player Id doesn't exist.");
            }

            // Create the TeamPlayer list with all Player data from the database
            var playerIds = teamPlayers.Select(tp => tp.PlayerId).ToArray();
            var players = await _context.TeamPlayer
                .Where(tp => tp.TeamId == teamId && playerIds.Contains(tp.PlayerId))
                .Include(tp => tp.Player)
                .ToListAsync();

            return CreatedAtAction("GetTeamPlayer", new { id = teamId }, new TeamPlayerListVm(players));
        }

        // DELETE: api/TeamPlayers/5
        [HttpDelete("{teamId}/{playerId}")]
        public async Task<IActionResult> DeleteTeamPlayer([FromRoute] int teamId,[FromRoute]int playerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(teamId <= 0 || playerId <= 0)
            {
                return NotFound("Combination of team Id and player Id does not exist");
            }


            TeamPlayer tp = new TeamPlayer();
            tp.PlayerId = playerId;
            tp.TeamId = teamId;

            var teamPlayer = await _context.TeamPlayer.Where(T => T.PlayerId == tp.PlayerId && T.TeamId == tp.TeamId).FirstOrDefaultAsync();
            if (teamPlayer == null)
            {
                return NotFound("Combination of team Id and player Id does not exist");
            
            }

            _context.TeamPlayer.Remove(teamPlayer);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool TeamPlayerExists(int teamId,int playerId)
        {
            return _context.TeamPlayer.Any(e => e.PlayerId == playerId && e.TeamId==teamId);
        }
    }
}