using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.Database.Data;
using EMS.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace EMS.BL.Repositories
{
    public interface IUserRepository
    {
        Task<List<UserModel>> GetUsers();
		Task<int> GetUsersAmount();
		Task<UserModel> GetUser(int id);
        Task<bool> ChangePassword(string username, string oldPassword, string newPassword);
        Task<bool> ToggleUserLockStatus(string username);
        Task<bool> UserModelExists(int id);
        Task<UserModel> CreateUser(UserModel userModel);
        Task DeleteUser(int id);
        Task UpdateUser(UserModel userModel);
        Task UpdateUserRoles(int userId, List<UserRoleModel> userRoles);

    }
    public class UserRepository(AppDbContext dbContext) : IUserRepository
    {
        public async Task<bool> ChangePassword(string username, string oldPassword, string newPassword)
        {
            // Tìm người dùng theo username
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

            // Xác thực mật khẩu cũ
            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.Password))
            {
                // Mật khẩu cũ không chính xác
                return false;
            }

            // Mã hóa mật khẩu mới
            string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);

            // Cập nhật mật khẩu mới
            user.Password = hashedNewPassword;

            // Lưu thay đổi vào cơ sở dữ liệu
            dbContext.Entry(user).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return true;
        }

        public Task<UserModel> CreateUser(UserModel userModel)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUser(int id)
        {
            var user = dbContext.Users.FirstOrDefault(n => n.ID == id);
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }

        public async Task<UserModel> GetUser(int id)
        {
            return await dbContext.Users.Include(n => n.UserRoles).ThenInclude(n => n.Role).FirstOrDefaultAsync(n => n.ID == id);
        }

        public async Task<List<UserModel>> GetUsers()
        {
            return await dbContext.Users.Include(n => n.UserRoles).ThenInclude(n => n.Role).ToListAsync();
        }

		public async Task<int> GetUsersAmount()
		{
            return await dbContext.Users.CountAsync();
		}

		public async Task<bool> ToggleUserLockStatus(string username)
        {
            // Tìm người dùng theo username
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);

            // Kiểm tra nếu người dùng không tồn tại
            if (user == null)
            {
                return false; // Hoặc bạn có thể ném ra một ngoại lệ tùy ý
            }

            // Đảo ngược trạng thái IsLock
            user.IsLock = !user.IsLock;

            // Cập nhật người dùng trong cơ sở dữ liệu
            dbContext.Entry(user).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return true;
        }

        public async Task UpdateUser(UserModel userModel)
        {
            dbContext.Entry(userModel).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateUserRoles(int userId, List<UserRoleModel> userRoles)
        {
            var existingRoles = dbContext.UserRoles.Where(ur => ur.UserID == userId);
            dbContext.UserRoles.RemoveRange(existingRoles);

            // Thêm các vai trò mới
            foreach (var role in userRoles)
            {
                dbContext.UserRoles.Add(new UserRoleModel { UserID = userId, RoleID = role.RoleID });
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> UserModelExists(int id)
        {
            return await dbContext.Users.AnyAsync(e => e.ID == id);
        }
    }
}
