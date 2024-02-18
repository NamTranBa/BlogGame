using DataAccess.Models;
using DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAL
{
    public class UserDAO
    {
        private readonly GameBlogDbContext _context;
        private readonly ILogger _logger;
        public UserDAO(GameBlogDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
            

        }
        public async Task<User> Register(UserDTO user)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user1 = new User
                {
                    Username = user.Username,
                    PasswordHash = PasswordHasher.HashPassword(user.password), // Sử dụng phương thức đã đề cập để hash mật khẩu
                    Email = user.Email,
                    CreateDate = DateTime.Now,
                    LastLogin = null,
                    IsBanned = false
                };
                await _context.Users.AddAsync(user1);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return user1;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while adding a new user");
                throw;
            }
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            var user = await _context.Users.Where(i => i.IsBanned == false).SingleOrDefaultAsync(c => c.Username.Equals(userName));
            return user;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            var user = await GetUserByUserName(username); // Giả sử phương thức này lấy đối tượng User từ cơ sở dữ liệu

            if (user != null && user.PasswordHash != null)
            {
                return PasswordHasher.VerifyPassword(user.PasswordHash, password);
            }
            return false;
        }

        public async Task<bool> ChangePassword(string userName, string oldPass, string newPass)
        {
            try
            {
                var user = await GetUserByUserName(userName);
                if (user == null || user.PasswordHash == null)
                {
                    // Người dùng không tồn tại hoặc có vấn đề với hash mật khẩu hiện tại
                    return false;
                }

                // Xác minh mật khẩu cũ
                if (!PasswordHasher.VerifyPassword(user.PasswordHash, oldPass))
                {
                    return false;
                }

                // Hash mật khẩu mới và cập nhật
                user.PasswordHash = PasswordHasher.HashPassword(newPass);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true; 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while changing the password for user {userName}", userName);
                throw; // Hoặc bạn có thể quyết định trả về false tùy vào cách xử lý lỗi của bạn
            }
        }

        public async Task<IEnumerable<User>> GetListUser()
        {
            var list = await _context.Users.ToListAsync();
            return list;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.UserId == id);
            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}
