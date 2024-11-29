using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMS.BL.Repositories;
using EMS.Model.Entities;

namespace EMS.BL.Services
{
    public interface IUserService
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
    public class UserService(IUserRepository userRepository) : IUserService
    {
        public Task<bool> ChangePassword(string username, string oldPassword, string newPassword)
        {
            return userRepository.ChangePassword(username, oldPassword, newPassword);
        }

        public Task<UserModel> CreateUser(UserModel userModel)
        {
            return userRepository.CreateUser(userModel);
        }

        public Task DeleteUser(int id)
        {
            return userRepository.DeleteUser(id);
        }

        public Task<UserModel> GetUser(int id)
        {
            return userRepository.GetUser(id);
        }

        public Task<List<UserModel>> GetUsers()
        {
            return userRepository.GetUsers();
        }

		public Task<int> GetUsersAmount()
		{
            return userRepository.GetUsersAmount();
		}

		public Task<bool> ToggleUserLockStatus(string username)
        {
            return userRepository.ToggleUserLockStatus(username);
        }

        public Task UpdateUser(UserModel userModel)
        {
            return userRepository.UpdateUser(userModel);
        }

        public Task UpdateUserRoles(int userId, List<UserRoleModel> userRoles)
        {
            return userRepository.UpdateUserRoles(userId, userRoles);
        }

        public Task<bool> UserModelExists(int id)
        {
            return userRepository.UserModelExists(id);
        }
    }
}
