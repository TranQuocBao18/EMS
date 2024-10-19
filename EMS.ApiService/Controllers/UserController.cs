using EMS.BL.Services;
using EMS.Model.Entities;
using EMS.Model.Models.Others;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMS.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<BaseResponseModel>> GetUsers()
        {
            var users = await userService.GetUsers();
            return Ok(new BaseResponseModel { Success = true, Data = users });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponseModel>> GetUser(int id)
        {
            var userModel = await userService.GetUser(id);

            if (userModel == null)
            {
                return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
            }
            return Ok(new BaseResponseModel { Success = true, Data = userModel });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!await userService.UserModelExists(id))
            {
                return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Not Found" });
            }
            await userService.DeleteUser(id);
            return Ok(new BaseResponseModel { Success = true });
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Gọi hàm ChangePassword trong UserServices
            var result = await userService.ChangePassword(request.Username, request.OldPassword, request.NewPassword);

            if (result)
            {
                return Ok(new { message = "Cập nhật mật khẩu thành công!" });
            }
            else
            {
                return BadRequest(new { message = "Mật khẩu không chính xác!" });
            }
        }

        [HttpPatch("ToggleUserLockStatus")]
        public async Task<IActionResult> ToggleUserLockStatus(string username)
        {
            var result = await userService.ToggleUserLockStatus(username);

            if (result)
            {
                return Ok(new { message = "User lock status updated successfully." });
            }
            else
            {
                return NotFound(new { message = "User not found." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserModel userModel)
        {
            if (id != userModel.ID || !await userService.UserModelExists(id))
            {
                return Ok(new BaseResponseModel { Success = false, ErrorMessage = "Bad request" });
            }

            await userService.UpdateUser(userModel);
            var userRolesList = userModel.UserRoles.ToList(); // Chuyển đổi ICollection thành List

            await userService.UpdateUserRoles(userModel.ID, userRolesList); // Sử dụng userRolesList
            return Ok(new BaseResponseModel { Success = true });
        }
    }
}
