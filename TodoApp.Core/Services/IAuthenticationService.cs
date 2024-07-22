using TodoApp.Core.DTOs;

namespace TodoApp.Core.Services
{
    public interface IAuthenticationService
    {
        Task<CustomResponseDto<UserAppDto>> RegisterAsync(UserCreateDto userCreateDto);
        Task<CustomResponseDto<AuthResponseDto>> LoginAsync(LoginDto loginDto);

    }
}