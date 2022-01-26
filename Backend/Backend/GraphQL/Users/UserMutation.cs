using Backend.DTOs;
using Backend.IRepository;
using Backend.Models;
using Backend.Repository;
using HotChocolate;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL.Users
{
    [ExtendObjectType(Name = "Mutation")]
    public class UserMutation
    {
        public async Task<UserWithToken> CreateUserAndToken(
            AddUserPayload userInput, [Service] IUserRepository userRepository,
            [Service] IIdentityRepository identityRepository)
        {
            User user = await userRepository.CreateUser(userInput);
            return new UserWithToken
            {
                User = user,
                Token = identityRepository.Authenticate(user.Email, user.Id)
            };
        }
    }
}
