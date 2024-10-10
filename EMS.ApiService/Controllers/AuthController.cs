﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EMS.BL.Services;
using EMS.Model.Entities;
using EMS.Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace EMS.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration, IAuthService authService) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseModel>> Login([FromBody] LoginModel loginModel)
        {
			var user = await authService.GetUserByLogin(loginModel.Username, loginModel.Password);
			if (user != null)
            {
                var token = GenerateJwtToken(user, isRefreshToken: false);
                var refreshToken = GenerateJwtToken(user, isRefreshToken: true);

				await authService.AddRefreshTokenModel(new RefreshTokenModel
				{
					RefreshToken = refreshToken,
					UserID = user.ID
				});

				return Ok(new LoginResponseModel { 
                    Token = token,
					TokenExpired = DateTimeOffset.UtcNow.AddHours(12).ToUnixTimeSeconds(),
					RefreshToken = refreshToken
                });
            }
            return null;
        }

		[HttpGet("loginByRefeshToken")]
		public async Task<ActionResult<LoginResponseModel>> LoginByRefeshToken(string refreshToken)
		{
			var refreshTokenModel = await authService.GetRefreshTokenModel(refreshToken);
			if (refreshTokenModel == null)
			{
				return StatusCode(StatusCodes.Status400BadRequest);
			}

			var newToken = GenerateJwtToken(refreshTokenModel.User, isRefreshToken: false);
			var newRefreshToken = GenerateJwtToken(refreshTokenModel.User, isRefreshToken: true);

			await authService.AddRefreshTokenModel(new RefreshTokenModel
			{
				RefreshToken = newRefreshToken,
				UserID = refreshTokenModel.UserID
			});

			return new LoginResponseModel
            {
                Token = newToken,
                TokenExpired = DateTimeOffset.UtcNow.AddHours(12).ToUnixTimeSeconds(),
                RefreshToken = newRefreshToken,
            };
		}
		private string GenerateJwtToken(UserModel user, bool isRefreshToken)
        {
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name, user.Username),
			};
			claims.AddRange(user.UserRoles.Select(n => new Claim(ClaimTypes.Role, n.Role.RoleName)));
			string secret = configuration.GetValue<string>($"Jwt:{(isRefreshToken ? "RefreshTokenSecret" : "Secret")}");
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "TranBao",
                audience: "TranBao",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(isRefreshToken ? 24 : 12),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}