using DataAccess.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAL
{
    public class CategoriesDAO
    {
        private readonly GameBlogDbContext _context;
        private readonly ILogger _logger;
        public CategoriesDAO(GameBlogDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Category> AddNewCategory(Category category)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return category;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while updating the category");
                throw;
            }
        }

        public async Task<Category> UpdateCategory(int id, Category categoryUpdate)
        {
            if (categoryUpdate == null) throw new ArgumentNullException(nameof(categoryUpdate));
            var cateogry = await _context.Categories.FindAsync(id);
            if (cateogry == null)
            {
                _logger.LogInformation($"Category with id {id} not found.");
                return null;
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Entry(cateogry).CurrentValues.SetValues(categoryUpdate);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return cateogry;
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while updating the category");
                throw;
            }
        }

        public async Task<Category> Remove(int id)
        {
            var cateogry = await _context.Categories.FindAsync(id);
            if (cateogry == null)
            {
                _logger.LogInformation($"Category with id {id} not found.");
                return null;
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Categories.Remove(cateogry);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return cateogry;
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while remove the category");
                throw;
            }
        }
    }
}
