using AutoMapper;
using Backend.Data;
using Backend.DTOs;
using Backend.IRepository;
using Backend.Models;
using HotChocolate;
using HotChocolate.Subscriptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Repository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly IMapper _mapper;
        public GroupRepository(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            IMapper mapper)
        {
            _mapper = mapper;
            _contextFactory = contextFactory;
        }
        public async Task<Group> CreateGroup(AddGroupPlayload groupInput, ITopicEventSender eventSender)
        {
            using(ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    var groupAdd = _mapper.Map<Group>(groupInput);
                    groupAdd.GroupUsers = new List<GroupUser>();
                    foreach (string userId in groupInput.GroupUserIds)
                    {
                        groupAdd.GroupUsers.Add(new GroupUser
                        {
                            UserId = userId
                        });
                    }

                    await context.Groups.AddAsync(groupAdd);
                    await context.SaveChangesAsync();

                    var groupAdded = await context.Groups
                        .Include("Host").Where(g => g.Id == groupAdd.Id)
                        .FirstOrDefaultAsync();

                    string groupCreatedTopic = "";
                    foreach(var id in groupInput.GroupUserIds)
                    {
                        groupCreatedTopic = $"{id}_{nameof(GraphQL.Groups.GroupSubscription.GroupCreated)}";
                        eventSender.SendAsync(groupCreatedTopic, groupAdded);
                    }

                    return groupAdded;
                }
                catch(Exception ex)
                {
                    throw new GraphQLException(new Error("Server errors", "SERVER_ERRORS"));
                }
            }
        }

        public async Task<List<ContactGroup>> GetContactGroups(string userId)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    List<ContactGroup> contactGroups = new List<ContactGroup>();
                    List<Message> messageLatestGroup = await context.Messages
                            .Where(m => m.Type == Models.Type.ToGroup && m.IsLatest == true &&
                            m.ToGroup.GroupUsers.Contains(new GroupUser()
                            {
                                GroupId = m.ToGroupId,
                                UserId = userId
                            })).Include("ToGroup").Include(g => g.ToGroup.GroupUsers).OrderBy(m => m.Date).ToListAsync();

                    foreach (var message in messageLatestGroup)
                    {
                        contactGroups.Add(new ContactGroup()
                        {
                            Group = message.ToGroup,
                            LatestMessage = message,
                            numOfMembers = message.ToGroup.GroupUsers.Count
                        });
                    }
                    return contactGroups;
                }
                catch (Exception ex)
                {
                    throw new GraphQLException(new Error("Server errors", "SERVER_ERRORS"));
                }
            }
        }
    }
}
