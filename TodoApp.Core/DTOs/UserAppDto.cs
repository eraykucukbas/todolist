using System;
using System.Collections.Generic;
using System.Text;

namespace TodoApp.Core.DTOs
{
    public class UserAppDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}