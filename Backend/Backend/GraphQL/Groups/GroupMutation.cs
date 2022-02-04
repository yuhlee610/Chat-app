using Backend.DTOs;
using Backend.IRepository;
using Backend.Models;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL.Groups
{
    [ExtendObjectType(Name = "Mutation")]
    [Authorize]
    public class GroupMutation
    {
        public async Task<ContactGroup> CreateGroup(
            AddGroupPlayload groupInput, [Service] IGroupRepository groupRepository,
            [Service] ITopicEventSender eventSender)
            => await groupRepository.CreateGroup(groupInput, eventSender);

        public async Task<ContactGroup> ExitGroup(string userId, string groupId,
            [Service] IGroupRepository groupRepository, [Service] ITopicEventSender eventSender)
            => await groupRepository.ExitGroup(userId, groupId, eventSender);

        public async Task<ContactGroup> RemoveMembers(
            List<string> userIds, string groupId, [Service] IGroupRepository groupRepository,
            [Service] ITopicEventSender eventSender)
            => await groupRepository.RemoveMembersFromGroup(userIds, groupId, eventSender);

        public async Task<ContactGroup> AddMembers(
            List<string> userIds, string groupId, [Service] IGroupRepository groupRepository,
            [Service] ITopicEventSender eventSender)
            => await groupRepository.AddMemberToGroup(userIds, groupId, eventSender);
    }
}
