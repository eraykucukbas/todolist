// using AutoMapper;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.Caching.Memory;
// using NLayer.Core;
// using NLayer.Core.DTOs;
// using NLayer.Core.Models;
// using NLayer.Core.Repositories;
// using NLayer.Core.Services;
// using NLayer.Core.UnitOfWorks;
// using NLayer.Service.Exceptions;
// using System.Linq.Expressions;
//
// namespace NLayer.Caching
// {
//     public class TodoListServiceWithCaching : ITodoListService
//     {
//         private const string CacheTodoListsKey = "todoListsCache";
//         private readonly IMapper _mapper;
//         private readonly IMemoryCache _memoryCache;
//         private readonly ITodoListRepository _repository;
//         private readonly IUnitOfWork _unitOfWork;
//
//         public TodoListServiceWithCaching(IUnitOfWork unitOfWork, ITodoListRepository repository, IMemoryCache memoryCache, IMapper mapper)
//         {
//             _unitOfWork = unitOfWork;
//             _repository = repository;
//             _memoryCache = memoryCache;
//             _mapper = mapper;
//
//             if (!_memoryCache.TryGetValue(CacheTodoListsKey, out _))
//             {
//                 CacheAllTodoListsAsync().Wait();
//             }
//         }
//
//         public async Task<TodoList> AddAsync(TodoList entity)
//         {
//             await _repository.AddAsync(entity);
//             await _unitOfWork.CommitAsync();
//             await CacheAllTodoListsAsync();
//             return entity;
//         }
//
//         public async Task<IEnumerable<TodoList>> AddRangeAsync(IEnumerable<TodoList> entities)
//         {
//             await _repository.AddRangeAsync(entities);
//             await _unitOfWork.CommitAsync();
//             await CacheAllTodoListsAsync();
//             return entities;
//         }
//
//         public async Task<bool> AnyAsync(Expression<Func<TodoList, bool>> expression)
//         {
//             return await _repository.AnyAsync(expression);
//         }
//
//         public Task<IEnumerable<TodoList>> GetAllAsync()
//         {
//             if (!_memoryCache.TryGetValue(CacheTodoListsKey, out IEnumerable<TodoList> todoLists))
//             {
//                 todoLists = _repository.GetAll().ToList();
//                 _memoryCache.Set(CacheTodoListsKey, todoLists);
//             }
//
//             return Task.FromResult(todoLists);
//         }
//
//         public Task<TodoList> GetByIdAsync(int id)
//         {
//             var todoList = _memoryCache.Get<List<TodoList>>(CacheTodoListsKey)?.FirstOrDefault(x => x.Id == id);
//
//             if (todoList == null)
//             {
//                 throw new NotFoundExcepiton($"{typeof(TodoList).Name}({id}) not found");
//             }
//
//             return Task.FromResult(todoList);
//         }
//
//         public Task<CustomResponseDto<List<TodoListWithTodoItemDto>>> GetTodoListsWithTodoItems()
//         {
//             var todoLists = _memoryCache.Get<IEnumerable<TodoList>>(CacheTodoListsKey);
//
//             var todoListWithTodoItemDto = _mapper.Map<List<TodoListWithTodoItemDto>>(todoLists);
//
//             return Task.FromResult(CustomResponseDto<List<TodoListWithTodoItemDto>>.Success(200, todoListWithTodoItemDto));
//         }
//
//         public async Task RemoveAsync(TodoList entity)
//         {
//             _repository.Remove(entity);
//             await _unitOfWork.CommitAsync();
//             await CacheAllTodoListsAsync();
//         }
//
//         public async Task RemoveRangeAsync(IEnumerable<TodoList> entities)
//         {
//             _repository.RemoveRange(entities);
//             await _unitOfWork.CommitAsync();
//             await CacheAllTodoListsAsync();
//         }
//
//         public async Task UpdateAsync(TodoList entity)
//         {
//             _repository.Update(entity);
//             await _unitOfWork.CommitAsync();
//             await CacheAllTodoListsAsync();
//         }
//
//         public IQueryable<TodoList> Where(Expression<Func<TodoList, bool>> expression)
//         {
//             return _memoryCache.Get<List<TodoList>>(CacheTodoListsKey)?.Where(expression.Compile()).AsQueryable();
//         }
//
//         public Task<CustomResponseDto<List<TodoListDto>>> GetAllIsCompleted(bool? isCompleted)
//         {
//             var todoLists = _memoryCache.Get<IEnumerable<TodoList>>(CacheTodoListsKey);
//
//             if (isCompleted.HasValue)
//             {
//                 todoLists = todoLists.Where(t => t.IsCompleted == isCompleted.Value);
//             }
//             
//             var todoListsDto = _mapper.Map<List<TodoListDto>>(todoLists.ToList());
//             var response = CustomResponseDto<List<TodoListDto>>.Success(200, todoListsDto);
//             return Task.FromResult(response);
//         } 
//         public Task<CustomResponseDto<List<TodoListWithTodoItemDto>>> GetByIdWithTodoItems(int id)
//         {
//             var todoList = _memoryCache.Get<IEnumerable<TodoListWithTodoItemDto>>(CacheTodoListsKey);
//             todoList = todoList
//                 .Where(t => t.Id == id)
//                 .Where(i => !i.IsDeleted)
//                 .OrderBy(i => i.IsCompleted)
//                 .ToList();
//       
//             var todoListDto = _mapper.Map<List<TodoListWithTodoItemDto>>(todoList.ToList());
//             var response = CustomResponseDto<List<TodoListWithTodoItemDto>>.Success(200, todoListDto);
//             return Task.FromResult(response);
//         }
//
//         private async Task CacheAllTodoListsAsync()
//         {
//             var todoLists = await _repository.GetAll().ToListAsync();
//             _memoryCache.Set(CacheTodoListsKey, todoLists);
//         }
//     }
// }