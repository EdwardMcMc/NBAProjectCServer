using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NbaDb.Data;
using NbaDb.Models;
using NbaSharedModels.RequestModels;
using NbaSharedModels.ViewModels;

namespace NbaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly NbaDbContext _context;

        public TeamsController(NbaDbContext context)
        {
            _context = context;
        }

        // GET: api/Teams
        [HttpGet]
        public async Task<TeamListVm> GetTeam()
        {
            var response = new TeamListVm();
            response.Teams = await _context.Team
                .Select(t => new TeamVm(t))
                .ToListAsync();
            return response;
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new TeamListVm();
            response.Teams = new List<TeamVm>();
            Team T = await _context.Team.FindAsync(id);
           
            if (T == null)
            {
                return NotFound($"No Teams exists with id: {id}");
            }
            response.Teams.Add(new TeamVm(T));
            return Ok(response);


            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //var team = await _context.Team.FindAsync(id);


            //if (team == null)
            //{
            //    return NotFound();
            //}

            //return Ok(team);
        }

        // PATCH: api/Teams/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTeam([FromRoute] int id, [FromBody] TeamReqModel teamReq)
        {
            // Final request model validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var team = teamReq.ConvertToTeamModel();
            team.Id = id;

            var existingTeam = await TeamExistsAsync(team.Name);
            if (existingTeam)
            {
                return BadRequest($"Team name: {team.Name} already exists.");
            }

            _context.Entry(team).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Teams
        [HttpPost]
        public async Task<IActionResult> PostTeam([FromBody] TeamReqModel teamReq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var team = teamReq.ConvertToTeamModel();

            var existingTeam = await TeamExistsAsync(team.Name);
            if (existingTeam)
            {
                return BadRequest($"Team name: {team.Name} already exists.");
            }

            _context.Team.Add(team);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTeam", new { id = team.Id }, new TeamVm(team));
        }

        // DELETE: api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var team = await _context.Team.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            _context.Team.Remove(team);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool TeamExists(int id)
        {
            return _context.Team.Any(e => e.Id == id);
        }

        private async Task<bool> TeamExistsAsync(string teamName)
        {
            return await _context.Team.AnyAsync(t => t.Name == teamName);
        }
    }
}