using Backend.DTOs;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.IRepository
{
    interface IUserRepository
    {
        Task<User> CreateUser(UserInputDTO user);
    }
}
