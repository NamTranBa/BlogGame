using DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAL
{
    public class GameDAO
    {
        private readonly GameBlogDbContext _context;
        private readonly ILogger _logger;
        public GameDAO(GameBlogDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Game>> GetAll(int pageNumber, int pageSize)
        {
            try
            {
                var games = await _context.Games
                                      .Skip((pageNumber - 1) * pageSize)
                                      .Take(pageSize)
                                      .AsNoTracking()
                                      .ToListAsync();
                return games;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;

            }
        }
        public async Task<IEnumerable<Game>> Search(string name)
        {
            try
            {
                var game = await _context.Games.Where(idg => idg.Title == name).ToListAsync();
                if (game == null)
                {
                    _logger.LogInformation($"Game with title {name} not found.");
                    return null;
                }
                return game;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        public async Task<Game> Get(int id)
        {
            try
            {
                var game = await _context.Games.SingleOrDefaultAsync(idg => idg.GameId == id);
                if (game == null)
                {
                    _logger.LogInformation($"Game with id {id} not found.");
                    return null;
                }
                return game;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            
        }

        public async Task<Game> AddNewGame(Game game)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Games.AddAsync(game);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return game;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while adding a new game");
                throw;
            }
        }
        
        public async Task<Game> UpdateGame (int id, Game gameUpdate)
        {
            if (gameUpdate == null) throw new ArgumentNullException(nameof(gameUpdate));
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                _logger.LogInformation($"Game with id {id} not found.");
                return null; 
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Entry(game).CurrentValues.SetValues(gameUpdate);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return game;
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while updating the game");
                throw;
            }
        }

        public async Task<Game> RemoveGame (int id)
        {
            
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                _logger.LogInformation($"Game with id {id} not found.");
                return null;
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Remove(game);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return game;
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while remove the game");
                throw;
            }
        }
    }
}
