using CornHoleRevamp.Data;
using CornHoleRevamp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CornHoleRevamp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameDataController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GameDataController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("Update/DB")]
        public async Task<ActionResult<IEnumerable<GameMatchResponseDto>>> UploadGameHistory([FromBody] JsonElement request)
        {
            try
            {
                // Log the incoming request for debugging
                Console.WriteLine($"Incoming request: {request.ToString()}");

                // Extract the data property from JsonElement
                if (!request.TryGetProperty("data", out JsonElement dataElement))
                {
                    return BadRequest(new { message = "Request must contain a 'data' property" });
                }

                // Convert JsonElement to string and then deserialize
                var dataString = dataElement.ToString();
                Console.WriteLine($"Data string: {dataString}");

                // Parse the JSON string to get the array of games
                var games = JsonSerializer.Deserialize<List<GameMatchDto>>(dataString);
                if (games == null)
                {
                    return BadRequest(new { message = "Invalid JSON data" });
                }

                Console.WriteLine($"Number of games to process: {games.Count}");

                var processedGames = new List<GameMatch>();

                foreach (var gameDto in games)
                {
                    Console.WriteLine($"Processing game: Date='{gameDto.Date}', Player1='{gameDto.Player1.Name}', Player2='{gameDto.Player2.Name}'");

                    // Safely parse the date - handle empty or invalid dates
                    DateTime playedAt = DateTime.UtcNow; // Default to current time
                    if (!string.IsNullOrEmpty(gameDto.Date) && DateTime.TryParse(gameDto.Date, out DateTime parsedDate))
                    {
                        playedAt = parsedDate;
                    }
                    else
                    {
                        Console.WriteLine($"Invalid date format for game, using current time: '{gameDto.Date}'");
                    }

                    // Validate that both players exist in the database
                    var player1 = await _context.Users.FindAsync(gameDto.Player1.Id);
                    if (player1 == null)
                    {
                        return BadRequest(new { message = $"Player1 with ID {gameDto.Player1.Id} does not exist in the database" });
                    }

                    var player2 = await _context.Users.FindAsync(gameDto.Player2.Id);
                    if (player2 == null)
                    {
                        return BadRequest(new { message = $"Player2 with ID {gameDto.Player2.Id} does not exist in the database" });
                    }

                    // Create GameMatch entity from DTO
                    var gameMatch = new GameMatch
                    {
                        Player1Id = gameDto.Player1.Id,
                        Player2Id = gameDto.Player2.Id,
                        Player1Name = gameDto.Player1.Name,  // Set from DTO
                        Player2Name = gameDto.Player2.Name,  // Set from DTO
                        Player1Score = gameDto.Player1.Score,  // Set from DTO
                        Player2Score = gameDto.Player2.Score,  // Set from DTO
                        BoardType = gameDto.Board,
                        TotalRounds = gameDto.TotalRounds,
                        PlayedAt = playedAt,
                        WinnerName = gameDto.Winner,  // Set from DTO
                        WinnerId = GetWinnerId(gameDto, gameDto.Player1.Id, gameDto.Player2.Id),
                        Player1 = player1,  // Assign the actual User object
                        Player2 = player2   // Assign the actual User object
                    };

                    Console.WriteLine($"Created game: {gameMatch.Player1Name} vs {gameMatch.Player2Name}, Score: {gameMatch.Player1Score} - {gameMatch.Player2Score}");

                    // Process rounds
                    foreach (var roundDto in gameDto.Rounds)
                    {
                        // Safely parse round timestamp
                        DateTime roundTimestamp = DateTime.UtcNow; // Default to current time
                        if (roundDto.Timestamp.HasValue && DateTime.TryParse(roundDto.Timestamp.Value.ToString(), out DateTime parsedRoundTime))
                        {
                            roundTimestamp = parsedRoundTime;
                        }
                        else
                        {
                            Console.WriteLine($"Invalid timestamp format for round, using current time: {roundDto.Timestamp}");
                        }

                        var round = new RoundData
                        {
                            RoundNumber = roundDto.RoundNumber,
                            Player1RoundScore = roundDto.Player1RoundScore,
                            Player2RoundScore = roundDto.Player2RoundScore,
                            Player1TotalBefore = roundDto.Player1TotalBefore,
                            Player2TotalBefore = roundDto.Player2TotalBefore,
                            Player1RoundBagsIn = roundDto.Player1RoundBagsIn,
                            Player1RoundBagsOn = roundDto.Player1RoundBagsOn,
                            Player2RoundBagsIn = roundDto.Player2RoundBagsIn,
                            Player2RoundBagsOn = roundDto.Player2RoundBagsOn,
                            Timestamp = roundTimestamp,
                            GameMatch = gameMatch
                        };

                        gameMatch.Rounds.Add(round);
                    }

                    processedGames.Add(gameMatch);
                }

                // Save all games
                _context.GameMatches.AddRange(processedGames);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Successfully saved {processedGames.Count} games to database");

                // Return the saved games in response format
                var response = processedGames.Select(g => new GameMatchResponseDto
                {
                    Id = g.Id,
                    Player1Id = g.Player1Id,
                    Player2Id = g.Player2Id,
                    Player1Name = g.Player1Name,
                    Player2Name = g.Player2Name,
                    Player1Score = g.Player1Score,
                    Player2Score = g.Player2Score,
                    WinnerName = g.WinnerName,
                    PlayedAt = g.PlayedAt,
                    BoardType = g.BoardType,
                    TotalRounds = g.TotalRounds,
                    Rounds = g.Rounds.Select(r => new RoundDataDto
                    {
                        RoundNumber = r.RoundNumber,
                        Player1RoundScore = r.Player1RoundScore,
                        Player2RoundScore = r.Player2RoundScore,
                        Player1TotalBefore = r.Player1TotalBefore,
                        Player2TotalBefore = r.Player2TotalBefore,
                        Player1RoundBagsIn = r.Player1RoundBagsIn,
                        Player1RoundBagsOn = r.Player1RoundBagsOn,
                        Player2RoundBagsIn = r.Player2RoundBagsIn,
                        Player2RoundBagsOn = r.Player2RoundBagsOn,
                        Timestamp = r.Timestamp
                    }).ToList()
                }).ToList();

                return Ok(response);
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON parsing error: {jsonEx.Message}");
                Console.WriteLine($"Stack trace: {jsonEx.StackTrace}");
                return BadRequest(new { message = $"Invalid JSON format: {jsonEx.Message}" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Server error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = $"Internal server error: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameMatchResponseDto>>> GetGameHistory()
        {
            var games = await _context.GameMatches
                .Include(g => g.Rounds)
                .ToListAsync();

            var response = games.Select(g => new GameMatchResponseDto
            {
                Id = g.Id,
                Player1Id = g.Player1Id,
                Player2Id = g.Player2Id,
                Player1Name = g.Player1Name,
                Player2Name = g.Player2Name,
                Player1Score = g.Player1Score,
                Player2Score = g.Player2Score,
                WinnerName = g.WinnerName,
                PlayedAt = g.PlayedAt,
                BoardType = g.BoardType,
                TotalRounds = g.TotalRounds,
                Rounds = g.Rounds.Select(r => new RoundDataDto
                {
                    RoundNumber = r.RoundNumber,
                    Player1RoundScore = r.Player1RoundScore,
                    Player2RoundScore = r.Player2RoundScore,
                    Player1TotalBefore = r.Player1TotalBefore,
                    Player2TotalBefore = r.Player2TotalBefore,
                    Player1RoundBagsIn = r.Player1RoundBagsIn,
                    Player1RoundBagsOn = r.Player1RoundBagsOn,
                    Player2RoundBagsIn = r.Player2RoundBagsIn,
                    Player2RoundBagsOn = r.Player2RoundBagsOn,
                    Timestamp = r.Timestamp
                }).ToList()
            }).ToList();

            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<GameMatchResponseDto>>> GetGamesByUser(int userId)
        {
            var games = await _context.GameMatches
                .Include(g => g.Rounds)
                .Where(g => g.Player1Id == userId || g.Player2Id == userId)
                .ToListAsync();

            var response = games.Select(g => new GameMatchResponseDto
            {
                Id = g.Id,
                Player1Id = g.Player1Id,
                Player2Id = g.Player2Id,
                Player1Name = g.Player1Name,
                Player2Name = g.Player2Name,
                Player1Score = g.Player1Score,
                Player2Score = g.Player2Score,
                WinnerName = g.WinnerName,
                PlayedAt = g.PlayedAt,
                BoardType = g.BoardType,
                TotalRounds = g.TotalRounds,
                Rounds = g.Rounds.Select(r => new RoundDataDto
                {
                    RoundNumber = r.RoundNumber,
                    Player1RoundScore = r.Player1RoundScore,
                    Player2RoundScore = r.Player2RoundScore,
                    Player1TotalBefore = r.Player1TotalBefore,
                    Player2TotalBefore = r.Player2TotalBefore,
                    Player1RoundBagsIn = r.Player1RoundBagsIn,
                    Player1RoundBagsOn = r.Player1RoundBagsOn,
                    Player2RoundBagsIn = r.Player2RoundBagsIn,
                    Player2RoundBagsOn = r.Player2RoundBagsOn,
                    Timestamp = r.Timestamp
                }).ToList()
            }).ToList();

            return Ok(response);
        }

        private int? GetWinnerId(GameMatchDto gameDto, int player1Id, int player2Id)
        {
            if (gameDto.Winner == "Tie") return null;
            if (gameDto.Winner == gameDto.Player1.Name) return player1Id;
            if (gameDto.Winner == gameDto.Player2.Name) return player2Id;
            return null;
        }
    }
}