using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using EMS.Database.Data;
using EMS.Model.Entities;
using EMS.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace EMS.BL.Repositories
{
	public interface IAuthRepository
	{
		Task<UserModel> GetUserByLogin(string username, string password);
		Task RemoveRefreshTokenByUserID(int userID);
		Task AddRefreshTokenModel(RefreshTokenModel refreshTokenModel);
		Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken);
		Task<UserModel> CreateUser(string username, string password, List<int> roleIds);
	}
	public class AuthRepository(AppDbContext dbContext) : IAuthRepository
	{
		public async Task<UserModel> GetUserByLogin(string username, string password)
		{
			// Tìm người dùng dựa trên username
			var user = await dbContext.Users.Include(n => n.UserRoles)
								   .ThenInclude(n => n.Role)
								   .FirstOrDefaultAsync(n => n.Username == username);

			// Nếu user tồn tại và mật khẩu khớp với mật khẩu đã được hash
			if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
			{
				return user; // Trả về user nếu mật khẩu đúng
			}

			return null;
		}
		public async Task RemoveRefreshTokenByUserID(int userID)
		{
			var refreshToken = dbContext.RefreshTokens.FirstOrDefault(n => n.UserID == userID);
			if (refreshToken != null)
			{
				dbContext.RemoveRange(refreshToken);
				await dbContext.SaveChangesAsync();
			}
		}
		public async Task AddRefreshTokenModel(RefreshTokenModel refreshTokenModel)
		{
			await dbContext.RefreshTokens.AddAsync(refreshTokenModel);
			await dbContext.SaveChangesAsync();
		}

		public async Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken)
		{
			return await dbContext.RefreshTokens.Include(n => n.User).ThenInclude(n => n.UserRoles).ThenInclude(n => n.Role).FirstOrDefaultAsync(n => n.RefreshToken == refreshToken);
		}
		public async Task<UserModel> CreateUser(string username, string password, List<int> roleIds)
		{
			var existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
			if (existingUser == null)
			{
				// Băm mật khẩu trước khi lưu vào cơ sở dữ liệu
				string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

				// Tạo đối tượng người dùng mới
				var newUser = new UserModel
				{
					Username = username,
					Password = hashedPassword,
					UserRoles = new List<UserRoleModel>()
				};

				// Gán các vai trò cho người dùng
				foreach (var roleId in roleIds)
				{
					var role = await dbContext.Roles.FindAsync(roleId);
					if (role != null)
					{
						newUser.UserRoles.Add(new UserRoleModel
						{
							RoleID = roleId,
							Role = role
						});
					}
				}

				// Lưu người dùng mới vào cơ sở dữ liệu
				dbContext.Users.Add(newUser);
				await dbContext.SaveChangesAsync();

				return newUser;
			}
			else { return null; }
		}
	}
}
