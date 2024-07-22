using TodoApp.Core.DTOs;
using TodoApp.Core.Models;

namespace TodoApp.Core.Services
{
    public interface IUserService
    {
        Task<CustomResponseDto<UserAppDto>> CreateUserAsync(UserCreateDto userCreateDto);
        Task<CustomResponseDto<NoContentDto>> UpdateUserAsync(string username,  UserUpdateDto userUpdateDto);
        Task<CustomResponseDto<NoContentDto>> ChangePassword(string username,  UserChangePasswordDto userChangePasswordDto);
        Task<CustomResponseDto<UserAppDto>> GetMyUser(string username);
        Task<CustomResponseDto<UserApp>> CheckCredentials(LoginDto loginDto);
        Task<CustomResponseDto<NoContentDto>> RemoveAsync(string username);

    }
}