using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NbaDb.Data;
using NbaDb.Models;
using NbaWebApi.ExceptionHandlers;
using NbaSharedModels.ViewModels;
using Microsoft.AspNetCore.Cors;

namespace NbaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class PlayersController : ControllerBase
    {
        private readonly NbaDbContext _context;

        public PlayersController(NbaDbContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<PlayerListVm> GetPlayer()
        {
            // return _context.Player;
            var response = new PlayerListVm();
            response.Players = await _context.Player
                .Select(p => new PlayerVm(p))
                .ToListAsync();
            return response;
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlayer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new PlayerListVm();
            response.Players= new List<PlayerVm>();
            Player P =await _context.Player.FindAsync(id);
            
            if (P == null)
            {
                return NotFound($"No players exists with playerId: {id}" );
            }
            response.Players.Add(new PlayerVm(P));
            return Ok(response);


            //var player = await _context.Player.FindAsync(id);

            //if (player == null)
            //{
            //    return NotFound();
            //}

            //return Ok(player);
        }

        // PUT: api/Players
        [HttpPut]
        public async Task<IActionResult> PutPlayer([FromBody] PlayerListVm playerList)
        {
            // Final validation of request model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var players = playerList.ConvertToListOfPlayerModels();
            foreach(Player p in players)
            {
                // Validate EF model
                TryValidateModel(p);
                if (!ModelState.IsValid)
                {
                    BadRequest(ModelState);
                }

                if (!PlayerExists(p.Id))
                {
                    return NotFound($"Player Id: {p.Id} doesn't exist.");
                } 
            }

            _context.Player.UpdateRange(players);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(playerList);
        }

        // POST: api/Players
        [HttpPost]
        public async Task<IActionResult> PostPlayer([FromBody] PlayerListVm playerList)
        {
            // Final validation of request model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (PlayerVm p in playerList.Players)
            {
                TryValidateModel(p);
                if (!ModelState.IsValid)
                {
                    BadRequest(ModelState);
                }
            }

            var players = playerList.ConvertToListOfPlayerModels();

            _context.Player.AddRange(players);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when ((ex.InnerException as SqlException)?.Number == 2627)
            {
                var message = SqlExceptionHandlers.DuplicatePrimaryKeyException(ex);
                return BadRequest(message);
            }

            var ids = players.Select(p => p.Id).ToArray();

            return CreatedAtAction("GetPlayer", new { id = ids }, playerList);
        }

        private IActionResult HandleSqlException(DbUpdateException ex)
        {
            throw new NotImplementedException();
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var player = await _context.Player.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Player.Remove(player);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool PlayerExists(int id)
        {
            return _context.Player.Any(e => e.Id == id);
        }
    }
}