using DataAccess.Model;
using DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        //User su dung
        public async Task<User> Register(string username, string password, string email)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user1 = new User
                {
                    Username = username,
                    PasswordHash = PasswordHasher.HashPassword(password), // Sử dụng phương thức đã đề cập để hash mật khẩu
                    Email = email,
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
                _logger.LogError(ex, $"An error occurred while changing the password for user {userName}");
                throw; // Hoặc bạn có thể quyết định trả về false tùy vào cách xử lý lỗi của bạn
            }
        }
        //Quan li User
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

        public async Task<User> Update(int id, User userUpdate)
        {
            if (userUpdate == null) throw new ArgumentNullException(nameof(userUpdate));
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogInformation($"User with id {id} not found.");
                return null;
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Entry(user).CurrentValues.SetValues(userUpdate);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return user;
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while updating the user");
                throw;
            }
        }
        public async Task<User> Remove(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogInformation($"User with id {id} not found.");
                return null;
            }
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return user;
            }
            catch (Exception ex)
            {

                await transaction.RollbackAsync();
                _logger.LogError(ex, "An error occurred while updating the user");
                throw;
            }
        }

    }
}
