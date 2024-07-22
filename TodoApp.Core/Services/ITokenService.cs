using System;
using System.Collections.Generic;
using System.Text;
using TodoApp.Core.Models;
using TodoApp.Core.DTOs;
using TodoApp.Core.Models;

namespace TodoApp.Core.Services
{
    public interface ITokenService
    {
        Task<TokenDto>  CreateToken(UserApp userApp);
    }
}