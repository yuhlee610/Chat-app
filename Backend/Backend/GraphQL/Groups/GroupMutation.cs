using Backend.DTOs;
using Backend.IRepository;
using Backend.Models;
using HotChocolate;
using HotChocolate.Subscriptions;
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
        public async Task<Group> CreateGroup(
            AddGroupPlayload groupInput, [Service] IGroupRepository groupRepository,
            [Service] ITopicEventSender eventSender)
            => await groupRepository.CreateGroup(groupInput, eventSender);
    }
}
