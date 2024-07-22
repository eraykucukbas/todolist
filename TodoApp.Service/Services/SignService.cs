using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Services
{
    public static class SignService
    {
        public static SecurityKey GetSymmetricSecurityKey(string securityKey)
        {
            if (string.IsNullOrEmpty(securityKey))
            {
                throw new ArgumentNullException(nameof(securityKey), "Security key cannot be null or empty");
            }

            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
            
        }
    }
}