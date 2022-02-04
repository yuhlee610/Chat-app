using Backend.DTOs;
using Backend.IRepository;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL.Groups
{
    [ExtendObjectType(Name = "Query")]
    [Authorize]
    public class GroupQuery
    {
        public async Task<List<ContactGroup>> GetContactGroups(
            string idUser, [Service] IGroupRepository groupRepository)
            => await groupRepository.GetContactGroups(idUser);
    }
}
