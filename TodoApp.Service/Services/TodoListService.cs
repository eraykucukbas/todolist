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
   public class TodoListService : Service<TodoList>, ITodoListService
{
    private readonly ITodoListRepository _todoListRepository;
    private readonly IMapper _mapper;

    public TodoListService(IGenericRepository<TodoList> repository, IUnitOfWork unitOfWork, IMapper mapper, ITodoListRepository todoListRepository) 
        : base(repository, unitOfWork)
    {
        _mapper = mapper;
        _todoListRepository = todoListRepository;
    }

    public async Task<CustomResponseDto<TodoList>> CheckUserAccess(int todoListId, string userId)
    {
        var todoList = await _todoListRepository.Where(t => t.Id == todoListId).FirstOrDefaultAsync(t => t.UserId == userId);
        if (todoList == null)
        {
            return CustomResponseDto<TodoList>.Fail(403, "You don't have access for this transaction");
        }
        return CustomResponseDto<TodoList>.Success(200, todoList);
    }

    public async Task<CustomResponseDto<List<TodoListDto>>> GetAllIsCompleted(bool? isCompleted, UserApp activeUser)
    {
        var todoLists = _todoListRepository.GetAll().Where(t => t.UserId == activeUser.Id);

        if (isCompleted.HasValue)
        {
            todoLists = todoLists.Where(t => t.IsCompleted == isCompleted.Value);
        }

        var todoListsDto = _mapper.Map<List<TodoListDto>>(await todoLists.ToListAsync());
        return CustomResponseDto<List<TodoListDto>>.Success(200, todoListsDto);
    }

    public async Task<CustomResponseDto<TodoListWithTodoItemDto>> GetByIdWithTodoItems(int id, UserApp activeUser)
    {
        var accessCheck = await CheckUserAccess(id, activeUser.Id);
        if (accessCheck.StatusCode == 403)
        {
            return CustomResponseDto<TodoListWithTodoItemDto>.Fail(accessCheck.StatusCode, accessCheck.Errors);
        }

        var todoList = await _todoListRepository
            .Where(t => t.Id == id)
            .Include(t => t.TodoItems)  // Include the TodoItems collection
            .FirstOrDefaultAsync();

        if (todoList == null)
        {
            return CustomResponseDto<TodoListWithTodoItemDto>.Fail(404, "Todo list not found");
        }

        var todoListDto = _mapper.Map<TodoListWithTodoItemDto>(todoList);
        return CustomResponseDto<TodoListWithTodoItemDto>.Success(200, todoListDto);
    }

    public async Task<CustomResponseDto<NoContentDto>> IsCompletedTrigger(int id, UserApp activeUser)
    {
        var accessCheck = await CheckUserAccess(id, activeUser.Id);
        if (accessCheck.StatusCode == 403)
        {
            return CustomResponseDto<NoContentDto>.Fail(accessCheck.StatusCode, accessCheck.Errors);
        }

        accessCheck.Data.IsCompleted = !accessCheck.Data.IsCompleted;
        await UpdateAsync(accessCheck.Data);
        return CustomResponseDto<NoContentDto>.Success(204);
    }

    public async Task<CustomResponseDto<TodoListDto>> CreateTodoList(TodoListCreateDto todoListCreateDto, UserApp activeUser)
    {
        var todoListCreateData = _mapper.Map<TodoList>(todoListCreateDto);
        todoListCreateData.UserId = activeUser.Id;
        var todoList = await AddAsync(todoListCreateData);
        var todoListDto = _mapper.Map<TodoListDto>(todoList);
        return CustomResponseDto<TodoListDto>.Success(201, todoListDto);
    }

    public async Task<CustomResponseDto<NoContentDto>> UpdateTodoList(TodoListUpdateDto todoListUpdateDto, UserApp activeUser)
    {
        var accessCheck = await CheckUserAccess(todoListUpdateDto.Id, activeUser.Id);
        if (accessCheck.StatusCode == 403)
        {
            return CustomResponseDto<NoContentDto>.Fail(accessCheck.StatusCode, accessCheck.Errors);
        }

        _mapper.Map(todoListUpdateDto, accessCheck.Data);
        await UpdateAsync(accessCheck.Data);
        return CustomResponseDto<NoContentDto>.Success(204);
    }

    public async Task<CustomResponseDto<NoContentDto>> RemoveTodoList(int id, UserApp activeUser)
    {
        var accessCheck = await CheckUserAccess(id, activeUser.Id);
        if (accessCheck.StatusCode == 403)
        {
            return CustomResponseDto<NoContentDto>.Fail(accessCheck.StatusCode, accessCheck.Errors);
        }

        await RemoveAsync(accessCheck.Data);
        return CustomResponseDto<NoContentDto>.Success(204);
    }
}
}