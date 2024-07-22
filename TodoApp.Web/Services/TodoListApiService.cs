using System.Net.Http.Headers;
using TodoApp.Core.DTOs;

namespace TodoApp.Web.Services
{
    public class TodoListApiService
    {
        private readonly HttpClient _httpClient;

        public TodoListApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<List<TodoListDto>> GetAllAsync(bool isCompleted = false, string token = "")
        {
            SetAuthorizationHeader(token);

            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<List<TodoListDto>>>($"todoLists?isCompleted={isCompleted}");
            return response?.Data ?? new List<TodoListDto>();
        }            
        
        public async Task<TodoListWithTodoItemDto?> GetByIdAsync(int id, string token = "")
        {
            SetAuthorizationHeader(token);

            var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<TodoListWithTodoItemDto>>($"todoLists/{id}");
            return response?.Data;
        }
        
        public async Task<TodoListDto?> Save(TodoListCreateDto newTodoList, string token = "")
        {
            SetAuthorizationHeader(token);

            var response = await _httpClient.PostAsJsonAsync("todoLists", newTodoList);

            if (!response.IsSuccessStatusCode) return null;

            var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<TodoListDto>>();

            return responseBody?.Data;
        }        
        
        public async Task<bool> Update(TodoListDto newTodoList, string token = "")
        {
            SetAuthorizationHeader(token);

            var response = await _httpClient.PutAsJsonAsync("todoLists", newTodoList);

            return response.IsSuccessStatusCode;
        }
        
        public async Task<bool> IsCompletedTrigger(IsCompletedTriggerDto dto, string token = "")
        {
            SetAuthorizationHeader(token);

            var response = await _httpClient.PutAsJsonAsync("todoLists/isCompletedTrigger", dto);
            return response.IsSuccessStatusCode;
        }
        
        
        public async Task<bool> Remove(int id, string token = "")
        {
            SetAuthorizationHeader(token);

            var response = await _httpClient.DeleteAsync($"todoLists/{id}");

            return response.IsSuccessStatusCode;
        }

    }
}