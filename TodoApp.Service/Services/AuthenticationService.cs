using System.Security.Cryptography;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TodoApp.Core.DTOs;
using TodoApp.Core.Models;
using TodoApp.Core.Repositories;
using TodoApp.Core.Services;
using TodoApp.Core.UnitOfWorks;

namespace TodoApp.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<UserApp> _userManager;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthenticationService( IMapper mapper, ITokenService tokenService,
            UserManager<UserApp> userManager, IUnitOfWork unitOfWork,
            IUserService userService)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<CustomResponseDto<UserAppDto>> RegisterAsync(UserCreateDto userCreateDto)
        {
            var user = await _userService.CreateUserAsync(userCreateDto);
            return CustomResponseDto<UserAppDto>.Success(200, user.Data);
        }

        public async Task<CustomResponseDto<AuthResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userService.CheckCredentials(loginDto);
            if (user.StatusCode == 404)
            {
                return CustomResponseDto<AuthResponseDto>.Fail(404, "user not found");
            }

            var token = await _tokenService.CreateToken(user.Data);
            var tokenDto = _mapper.Map<AuthResponseDto>(token);
            var userToken = new IdentityUserToken<string>
            {
                UserId = user.Data.Id,
                LoginProvider = "TodoApp",
                Name = "RefreshToken",
                Value = tokenDto.RefreshToken
            };
        
            await _userManager.SetAuthenticationTokenAsync(user.Data, userToken.LoginProvider, userToken.Name, userToken.Value);
            
            return CustomResponseDto<AuthResponseDto>.Success(200, tokenDto);
        }
    }
}