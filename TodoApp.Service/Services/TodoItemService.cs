using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoApp.Core;
using TodoApp.Core.DTOs;
using TodoApp.Core.Models;
using TodoApp.Core.Repositories;
using TodoApp.Core.Services;
using TodoApp.Core.UnitOfWorks;

namespace TodoApp.Service.Services
{
    public class TodoItemService : Service<TodoItem>, ITodoItemService
    {
        private readonly ITodoItemRepository _todoItemRepository;
        private readonly IMapper _mapper;
        private readonly ITodoListService _todoListService;

        public TodoItemService(IGenericRepository<TodoItem> repository, IUnitOfWork unitOfWork, IMapper mapper, ITodoItemRepository todoItemRepository,ITodoListService todoListService) 
            : base(repository, unitOfWork)
        {
            _mapper = mapper;
            _todoItemRepository = todoItemRepository;
            _todoListService = todoListService;
        }

        private async Task<CustomResponseDto<TodoItem>> CheckUserAccess(int todoItemId, string userId)
        {
            var todoItem = await _todoItemRepository
                .Where(t => t.Id == todoItemId)
                .Include(t => t.TodoList)
                .FirstOrDefaultAsync();

            if (todoItem == null)
            {
                return CustomResponseDto<TodoItem>.Fail(404, "TodoItem not found");
            }

            if (todoItem.TodoList.UserId != userId)
            {
                return CustomResponseDto<TodoItem>.Fail(403, "You don't have access for this transaction");
            }

            return CustomResponseDto<TodoItem>.Success(200, todoItem);
        }

        public async Task<CustomResponseDto<TodoItemDto>> GetTodoItemByIdAsync(int id, UserApp activeUser)
        {
            var accessCheck = await CheckUserAccess(id, activeUser.Id);
            if (accessCheck.StatusCode != 200)
            {
                return CustomResponseDto<TodoItemDto>.Fail(accessCheck.StatusCode, accessCheck.Errors);
            }

            var todoItemDto = _mapper.Map<TodoItemDto>(accessCheck.Data);
            return CustomResponseDto<TodoItemDto>.Success(200, todoItemDto);
        }

        public async Task<CustomResponseDto<TodoItemDto>> AddTodoItemAsync(TodoItemCreateDto todoItemCreateDto, UserApp activeUser)
        {
            
            var accessCheck = await _todoListService.CheckUserAccess(todoItemCreateDto.TodoListId, activeUser.Id);
            if (accessCheck.StatusCode != 200)
            {
                return CustomResponseDto<TodoItemDto>.Fail(accessCheck.StatusCode, accessCheck.Errors);
            }
            var todoItem = _mapper.Map<TodoItem>(todoItemCreateDto);

            var addedTodoItem = await AddAsync(todoItem);
            var todoItemDto = _mapper.Map<TodoItemDto>(addedTodoItem);
            return CustomResponseDto<TodoItemDto>.Success(201, todoItemDto);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateTodoItem(TodoItemUpdateDto todoItemUpdateDto, UserApp activeUser)
        {
            var accessCheck = await CheckUserAccess(todoItemUpdateDto.Id, activeUser.Id);
            if (accessCheck.StatusCode != 200)
            {
                return CustomResponseDto<NoContentDto>.Fail(accessCheck.StatusCode, accessCheck.Errors);
            }

            _mapper.Map(todoItemUpdateDto, accessCheck.Data);
            await UpdateAsync(accessCheck.Data);
            return CustomResponseDto<NoContentDto>.Success(204);
        }

        public async Task<CustomResponseDto<NoContentDto>> IsCompletedTrigger(int id, UserApp activeUser)
        {
            var accessCheck = await CheckUserAccess(id, activeUser.Id);
            if (accessCheck.StatusCode != 200)
            {
                return CustomResponseDto<NoContentDto>.Fail(accessCheck.StatusCode, accessCheck.Errors);
            }

            accessCheck.Data.IsCompleted = !accessCheck.Data.IsCompleted;
            await UpdateAsync(accessCheck.Data);
            return CustomResponseDto<NoContentDto>.Success(204);
        }

        public async Task<CustomResponseDto<NoContentDto>> RemoveTodoItemAsync(int id, UserApp activeUser)
        {
            var accessCheck = await CheckUserAccess(id, activeUser.Id);
            if (accessCheck.StatusCode != 200)
            {
                return CustomResponseDto<NoContentDto>.Fail(accessCheck.StatusCode, accessCheck.Errors);
            }

            await RemoveAsync(accessCheck.Data);
            return CustomResponseDto<NoContentDto>.Success(204);
        }
    }
}