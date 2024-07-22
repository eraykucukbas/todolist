using Microsoft.AspNetCore.Http.HttpResults;
using TodoApp.Core.DTOs;

namespace TodoApp.Web.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomResponseDto<UserAppDto>> Register(UserCreateDto userCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/register", userCreateDto);
            return await response.Content.ReadFromJsonAsync<CustomResponseDto<UserAppDto>>();
        }

        
        public async Task<AuthResponseDto?> Login(LoginDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", loginDto);
            
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
                var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<AuthResponseDto>>();
                return responseBody?.Data;
        }
    }
}