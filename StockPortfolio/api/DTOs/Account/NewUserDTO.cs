using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Account
{
    public class NewUserDTO
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? token { get; set; }
    }
}