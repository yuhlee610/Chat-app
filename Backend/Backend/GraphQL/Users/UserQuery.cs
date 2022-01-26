using Backend.Data;
using Backend.DTOs;
using Backend.IRepository;
using Backend.Models;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.GraphQL.Users
{
    [ExtendObjectType(Name = "Query")]
    public class UserQuery
    {
        //[Authorize]
        public async Task<List<ContactUser>> GetContactUsers(
            string idUser, [Service] IUserRepository userRepository)
        {
            return await userRepository.GetContactUsers(idUser);
        }

        public async Task<List<User>> GetUsers([Service] IUserRepository userRepository)
        => await userRepository.GetUsers();
    }
}
