using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs
{
    public class UserWithToken
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
