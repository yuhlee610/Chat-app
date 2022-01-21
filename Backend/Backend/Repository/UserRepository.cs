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
                    //List<Message> messagesBetweenUser = await context.Messages
                    //    .Where(m => (m.SendByUserId == userId || m.ToUserId == userId) && m.Type == Models.Type.ToUser)
                    //    .Include("ToUser").Include("SendByUser")
                    //    .ToListAsync();
                    //List<User> contactedUser = new List<User>();
                    //foreach (var message in messagesBetweenUser)
                    //{
                    //    if (!contactedUser.Any(u => u.Id == message.ToUser.Id) && message.ToUser.Id != userId)
                    //    {
                    //        contactedUser.Add(message.ToUser);
                    //    }
                    //    if (!contactedUser.Any(u => u.Id == message.SendByUser.Id) && message.SendByUser.Id != userId)
                    //    {
                    //        contactedUser.Add(message.SendByUser);
                    //    }
                    //}
                    List<String> messagesBetweenUserId = await (from m in context.Messages
                                                        where m.Type == Models.Type.ToUser && m.SendByUserId == userId
                                                        select m.ToUserId)
                                                        .Union(from m in context.Messages
                                                               where m.Type == Models.Type.ToUser && m.ToUserId == userId
                                                               select m.SendByUserId).ToListAsync();

                    List<User> contactedUsers = await context.Users
                        .Where(u => messagesBetweenUserId.Contains(u.Id))
                        .ToListAsync();

                    List<Group> contactedGroups = await context.Groups
                        .Where(g => g.GroupUsers.Where(gu => gu.UserId == userId).Any())
                        .ToListAsync();

                    return new GroupAndUserContacted() { 
                        ContactedUsers = contactedUsers,
                        ContactedGroups = contactedGroups
                    };
                }
                catch (Exception ex)
                {
                    throw new GraphQLException(new Error("Server errors", "SERVER_ERRORS"));
                }
            }
        }
    }
}
