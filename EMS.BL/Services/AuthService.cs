using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.BL.Repositories;
using EMS.Model.Entities;

namespace EMS.BL.Services
{
	public interface IAuthService
	{
		Task<UserModel> GetUserByLogin(string username, string password);
		Task AddRefreshTokenModel(RefreshTokenModel refreshTokenModel);
		Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken);
		Task<UserModel> CreateUser(string username, string password, List<int> roleIds);
	}

	public class AuthService(IAuthRepository authRepository) : IAuthService
	{
		public Task<UserModel> GetUserByLogin(string username, string password)
		{
			return authRepository.GetUserByLogin(username, password);
		}
		public async Task AddRefreshTokenModel(RefreshTokenModel refreshTokenModel)
		{
			await authRepository.RemoveRefreshTokenByUserID(refreshTokenModel.UserID);
			await authRepository.AddRefreshTokenModel(refreshTokenModel);
		}
		public Task<RefreshTokenModel> GetRefreshTokenModel(string refreshToken)
		{
			return authRepository.GetRefreshTokenModel(refreshToken);
		}

		public Task<UserModel> CreateUser(string username, string password, List<int> roleIds)
		{
			return authRepository.CreateUser(username, password, roleIds);
		}
	}
}
