using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TodoApp.Core.DTOs
{
    public class UserChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}