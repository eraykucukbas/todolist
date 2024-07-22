using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Core.Models
{
    public class UserApp : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        
        public ICollection<TodoList> TodoLists { get; set; }

    }
}