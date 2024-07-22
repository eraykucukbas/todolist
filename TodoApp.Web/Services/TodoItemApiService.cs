using TodoApp.Core.DTOs;
using System.Net.Http.Headers;

public class TodoItemApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TodoItemApiService> _logger;

    public TodoItemApiService(HttpClient httpClient, ILogger<TodoItemApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    private void SetAuthorizationHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<TodoItemDto> Save(TodoItemCreateDto newTodoItem, string token)
    {
        SetAuthorizationHeader(token);
        var response = await _httpClient.PostAsJsonAsync("todoItems", newTodoItem);

        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadFromJsonAsync<CustomResponseDto<TodoItemDto>>();
        return responseBody.Data;
    }

    public async Task<TodoItemDto> GetByIdAsync(int id, string token)
    {
        SetAuthorizationHeader(token);
        var response = await _httpClient.GetFromJsonAsync<CustomResponseDto<TodoItemDto>>($"todoItems/{id}");
        return response.Data;
    }
    
    public async Task<bool> Update(TodoItemDto newTodoItem, string token)
    {
        SetAuthorizationHeader(token);
        var response = await _httpClient.PutAsJsonAsync("todoItems", newTodoItem);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> IsCompletedTrigger(IsCompletedTriggerDto dto, string token)
    {
        SetAuthorizationHeader(token);
        var response = await _httpClient.PutAsJsonAsync("todoItems/isCompletedTrigger", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> Remove(int id, string token)
    {
        SetAuthorizationHeader(token);
        _logger.LogInformation($"Attempting to delete todo item with ID: {id}");
        var response = await _httpClient.DeleteAsync($"todoItems/{id}");
        return response.IsSuccessStatusCode;
    }
}