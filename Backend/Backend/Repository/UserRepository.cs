using AutoMapper;
using Backend.Data;
using Backend.DTOs;
using Backend.IRepository;
using Backend.Models;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly IMapper _mapper;
        public UserRepository(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            IMapper mapper)
        {
            _mapper = mapper;
            _contextFactory = contextFactory;
        }
        public async Task<User> CreateUser(UserInputDTO userInput)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    if (await context.Users.AnyAsync(u => u.Email == userInput.Email))
                    {
                        throw new GraphQLException(new Error("Account exists", "ACCOUNT_EXISTS"));
                    }

                    var userAdd = _mapper.Map<User>(userInput);
                    await context.AddAsync(userAdd);
                    await context.SaveChangesAsync();
                    return userAdd;
                }
                catch
                {
                    throw new GraphQLException(new Error("Server errors", "SERVER_ERRORS"));
                }
            }
        }

        public async Task<GroupAndUserContacted> GetContactedUser(string userId)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    GroupAndUserContacted result = new GroupAndUserContacted();

                    List<Message> messageLatestUser = await context.Messages
                        .Where(m => m.IsLatest == true && m.Type == Models.Type.ToUser && (
                        m.SendByUserId == userId || m.ToUserId == userId)).OrderBy(m => m.Date)
                        .ToListAsync();

                    foreach(var message in messageLatestUser)
                    {
                        User user = new User();
                        if(message.ToUserId != userId)
                        {
                            user = await context.Users.FindAsync(message.ToUserId);
                        }
                        if(message.SendByUserId != userId)
                        {
                            user = await context.Users.FindAsync(message.SendByUserId);
                        }
                        result.ContactedUsers.Add(new UserContacted()
                        {
                            User = user,
                            LatestMessage = message
                        });
                    }

                    List<Message> messageLatestGroup = await context.Messages
                        .Where(m => m.Type == Models.Type.ToGroup && m.IsLatest == true &&
                        m.ToGroup.GroupUsers.Contains(new GroupUser()
                        {
                            GroupId = m.ToGroupId,
                            UserId = userId
                        })).Include("ToGroup").OrderBy(m => m.Date).ToListAsync();

                    foreach(var message in messageLatestGroup)
                    {
                        result.ContactedGroups.Add(new GroupContacted()
                        {
                            Group = message.ToGroup,
                            LatestMessage = message
                        });
                    }

                    return result;
                }
                catch (Exception ex)
                {
                    throw new GraphQLException(new Error("Server errors", "SERVER_ERRORS"));
                }
            }
        }
    }
}
