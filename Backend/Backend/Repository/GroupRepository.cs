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
        public async Task<ContactGroup> CreateGroup(
            AddGroupPlayload groupInput, ITopicEventSender eventSender)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
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
                        .Include("Host").Include("GroupUsers").Where(g => g.Id == groupAdd.Id)
                        .FirstOrDefaultAsync();

                    ContactGroup newContactGroup = new ContactGroup
                    {
                        Group = groupAdded,
                        LatestMessage = new Message { Content = "", Date = DateTime.UtcNow },
                        numOfMembers = groupAdd.GroupUsers.Count
                    };

                    string groupCreatedTopic = "";
                    foreach (var id in groupInput.GroupUserIds)
                    {
                        groupCreatedTopic = $"{id}_{nameof(GraphQL.Groups.GroupSubscription.GroupCreated)}";
                        await eventSender.SendAsync(groupCreatedTopic, newContactGroup);
                    }

                    return newContactGroup;
                }
                catch (Exception ex)
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

                    List<Group> groups = await context.GroupUsers.Where(gu => gu.UserId == userId)
                        .Select(gu => gu.Group).ToListAsync();

                    foreach (Group group in groups)
                    {
                        Message latestMessageOfGroup = await context.Messages
                                .Where(m => m.Type == Models.Type.ToGroup && m.IsLatest == true && m.ToGroupId == group.Id)
                                .FirstOrDefaultAsync();
                        contactGroups.Add(new ContactGroup
                        {
                            Group = await context.Groups.Where(g => g.Id == group.Id).Include("Host").FirstOrDefaultAsync(),
                            LatestMessage = latestMessageOfGroup != null ? latestMessageOfGroup : new Message { Content = "", Date = DateTime.UtcNow },
                            numOfMembers = await context.GroupUsers.Where(gu => gu.GroupId == group.Id).CountAsync()
                        });
                    }

                    return contactGroups.OrderByDescending(cg => cg.LatestMessage.Date).ToList();
                }
                catch (Exception ex)
                {
                    throw new GraphQLException(new Error("Server errors", "SERVER_ERRORS"));
                }
            }
        }
    }
}
