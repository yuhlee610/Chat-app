using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
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
        private readonly UserRepository _userRepository;
        public UserMutation(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> CreateUser(UserInputDTO userInput) => 
            await _userRepository.CreateUser(userInput);
    }
}
