using Backend.Models;
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
        public User GetUser() => new User();
    }
}
