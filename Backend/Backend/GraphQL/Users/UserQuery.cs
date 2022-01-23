using Backend.Data;
using Backend.IRepository;
using Backend.Models;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL.Users
{
    [ExtendObjectType(Name = "Query")]
    public class UserQuery
    {
        private readonly IUserRepository _userRepository;
        public UserQuery(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<GroupAndUserContacted> GetContactedUsersAndGroups(string idUser)
            => await _userRepository.GetContactedUser(idUser);
    }
}
