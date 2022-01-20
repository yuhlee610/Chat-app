using Backend.DTOs;
using Backend.IRepository;
using Backend.Models;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL.Groups
{
    [ExtendObjectType(Name = "Mutation")]
    public class GroupMutation
    {
        private readonly IGroupRepository _groupRepository;
        public GroupMutation(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task<Group> CreateGroup(GroupInputDTO groupInput)
            => await _groupRepository.CreateGroup(groupInput);
    }
}
