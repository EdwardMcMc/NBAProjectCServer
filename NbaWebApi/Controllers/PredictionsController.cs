using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NbaDb.Data;
using NbaDb.Models;
using NbaSharedModels.ViewModels;
using NbaWebApi.Models;
using NbaWebApi.Services;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;

namespace NbaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class PredictionsController : ControllerBase
    {
        private readonly int MAX_PLAYERS = 15;
        private readonly NbaDbContext _context;
        private readonly string ApiKey = @"ImWNFuOOr205x9bHV1oEEOhBJG3Vehnlyx8wjyhJgTE3ZvKQCPnyKPthwczF7Iv5wslU+W251cUN3VO+uIvxuA==";
        private readonly string Url = @"https://ussouthcentral.services.azureml.net/workspaces/10ab5c365f0f41c69ebe26a366f7d2bd/services/082b941849aa4cf9bf87b9ff0563a425/execute?api-version=2.0&format=swagger";

        public PredictionsController(NbaDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = new TeamPredictionList();
            response.Predictions = new List<TeamPrediction>();
            Team T = await _context.Team.FindAsync(id);
            if (T == null)
            {
                return NotFound($"No Teams exists with id: {id}");
            }
            response.Predictions.Add(new TeamPrediction(T));
            return Ok(response);

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = new TeamPredictionList();
            response.Predictions = await _context.Team
                .Select(t => new TeamPrediction(t))
                .ToListAsync();
            if (response.Predictions.Count == 0)
            {
                return NotFound("No Teams Found");
            }
            return Ok(response);
        }
    }
}
