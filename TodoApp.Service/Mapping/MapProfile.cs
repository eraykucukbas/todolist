using AutoMapper;
using TodoApp.Core.DTOs;
using TodoApp.Core.Models;
using TodoApp.Core.Models;

namespace TodoApp.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<BaseDto, TodoList>().ReverseMap();
            CreateMap<TodoItem, TodoItemDto>().ReverseMap();
            CreateMap<TodoList, TodoListDto>().ReverseMap();
            CreateMap<TodoListCreateDto, TodoList>().ReverseMap();
            CreateMap<TodoListUpdateDto, TodoList>();
            CreateMap<TodoItemUpdateDto, TodoItem>().ReverseMap();
            CreateMap<TodoItemCreateDto, TodoItem>().ReverseMap();
            CreateMap<TodoList, TodoListWithTodoItemDto>().ReverseMap();
            CreateMap<UserAppDto, UserApp>().ReverseMap();
            CreateMap<UserUpdateDto, UserApp>().ReverseMap();
            CreateMap<TokenDto, AuthResponseDto>().ReverseMap();

        }
    }
}