using FluentValidation;
using TodoApp.Core.DTOs;

namespace TodoApp.Service.Validations
{
    public class TodoItemDtoValidator : AbstractValidator<TodoItemDto>
    {
        public TodoItemDtoValidator()
        {
            RuleFor(x => x.Title).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.Description).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
        }
    }
}
