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
        public async Task<User> CreateUser(AddUserPayload userInput)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    if (await context.Users.AnyAsync(u => u.Email == userInput.Email))
                    {
                        return await context.Users.Where(u => u.Email == userInput.Email).FirstOrDefaultAsync();
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

        public async Task<List<ContactUser>> GetContactUsers(string userId)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    List<ContactUser> contactUsers = new List<ContactUser>();

                    List<Message> messageLatestUser = await context.Messages
                            .Where(m => m.IsLatest == true && m.Type == Models.Type.ToUser && (
                            m.SendByUserId == userId || m.ToUserId == userId)).OrderBy(m => m.Date)
                            .ToListAsync();
                    foreach (var message in messageLatestUser)
                    {
                        User user = new User();
                        if (message.ToUserId != userId)
                        {
                            user = await context.Users.FindAsync(message.ToUserId);
                        }
                        if (message.SendByUserId != userId)
                        {
                            user = await context.Users.FindAsync(message.SendByUserId);
                        }
                        contactUsers.Add(new ContactUser()
                        {
                            User = user,
                            LatestMessage = message
                        });
                    }
                    return contactUsers;
                }
                catch (Exception ex)
                {
                    throw new GraphQLException(new Error("Server errors", "SERVER_ERRORS"));
                }
            }
        }

        public async Task<List<User>> GetUsers()
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    return await context.Users.ToListAsync();
                }
                catch (Exception ex)
                {
                    throw new GraphQLException(new Error("Server errors", "SERVER_ERRORS"));
                }
            }
        }
    }
}
