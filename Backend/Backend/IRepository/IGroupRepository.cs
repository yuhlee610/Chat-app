using Backend.DTOs;
using Backend.Models;
using HotChocolate.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.IRepository
{
    public interface IGroupRepository
    {
        Task<ContactGroup> CreateGroup(AddGroupPlayload groupInput, ITopicEventSender eventSender);
        Task<List<ContactGroup>> GetContactGroups(string userId);
        Task<ContactGroup> ExitGroup(string userId, string groupId, ITopicEventSender eventSender);
        Task<ContactGroup> AddMemberToGroup(List<string> userIds, string groupId, ITopicEventSender eventSender);
        Task<ContactGroup> RemoveMembersFromGroup(List<string> userIds, string groupId, ITopicEventSender eventSender);
    }
}
