using TodoApp.Core.DTOs;
using System.Net.Http.Headers;

namespace TodoApp.Web.Services
{
    public class UserApiService
    {
        private readonly HttpClient _httpClient;

        public UserApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<UserAppDto> GetMyUserAsync(string token)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<UserAppDto>>("user");
            return response.Data;
        }

        public async Task<bool> UpdateUserAsync(UserUpdateDto userUpdateDto, string token)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.PutAsJsonAsync("user", userUpdateDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChangePasswordAsync(UserChangePasswordDto userChangePasswordDto, string token)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.PutAsJsonAsync("user/changePassword", userChangePasswordDto);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveUserAsync(string token)
        {
            SetAuthorizationHeader(token);
            var response = await _httpClient.DeleteAsync("user");
            return response.IsSuccessStatusCode;
        }
    }
}