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
        Task<Group> CreateGroup(AddGroupPlayload groupInput, ITopicEventSender eventSender);
        Task<List<ContactGroup>> GetContactGroups(string userId);
    }
}
