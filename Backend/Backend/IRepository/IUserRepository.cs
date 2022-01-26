using Backend.DTOs;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.IRepository
{
    public interface IUserRepository
    {
        Task<User> CreateUser(AddUserPayload user);
        Task<List<ContactUser>> GetContactUsers(string userId);
        Task<List<User>> GetUsers();
    }
}
