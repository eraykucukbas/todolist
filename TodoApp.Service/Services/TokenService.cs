using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TodoApp.Core.DTOs;
using TodoApp.Core.Models;
using TodoApp.Core.Services;
using TodoApp.Repository.Configurations;
using TodoApp.Services;
using static System.DateTime;

namespace TodoApp.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<UserApp> _userManager;

        private readonly CustomTokenOption _tokenOption;

        public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOption> options)
        {
            _userManager = userManager;
            _tokenOption = options.Value;
        }

        private string CreateRefreshToken()

        {
            var numberByte = new Byte[32];

            using var rnd = RandomNumberGenerator.Create();

            rnd.GetBytes(numberByte);

            return Convert.ToBase64String(numberByte);
        }
        

        public Task<TokenDto> CreateToken(UserApp userApp)
        {
            var accessTokenExpiration = Now.AddMinutes(_tokenOption.AccessTokenExpiration);
            var refreshTokenExpiration = Now.AddMinutes(_tokenOption.RefreshTokenExpiration);
            var securityKey = SignService.GetSymmetricSecurityKey(_tokenOption.SecurityKey);

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userApp.Id.ToString()),
                new Claim(ClaimTypes.Name, userApp.UserName)

            };
            
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _tokenOption.Issuer,
                audience: _tokenOption.Audience[0],
                claims: claims,
                expires: accessTokenExpiration,
                notBefore: Now,
                signingCredentials: signingCredentials);

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtSecurityToken);

            var tokenDto = new TokenDto
            {
                AccessToken = token,
                RefreshToken = CreateRefreshToken(),
                AccessTokenExpiration = accessTokenExpiration,
                RefreshTokenExpiration = refreshTokenExpiration
            };
            return Task.FromResult(tokenDto);
        }
    }
}