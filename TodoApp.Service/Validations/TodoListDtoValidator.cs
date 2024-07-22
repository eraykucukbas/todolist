using FluentValidation;
using TodoApp.Core.DTOs;

namespace TodoApp.Service.Validations
{
    public class TodoListDtoValidator : AbstractValidator<TodoListDto>
    {
        public TodoListDtoValidator()
        {

            RuleFor(x => x.Title).NotNull().WithMessage("{PropertyName} is required").NotEmpty().WithMessage("{PropertyName} is required");
            
        }


    }
}
