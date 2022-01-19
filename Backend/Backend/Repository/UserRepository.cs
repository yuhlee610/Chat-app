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
        public async Task<User> CreateUser(UserInputDTO user)
        {
            using(ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    if (await context.Users.AnyAsync(u => u.Email == user.Email))
                    {
                        throw new GraphQLException(new Error("Account exists", "ACCOUNT_EXISTS"));
                    }

                    var userAdd = _mapper.Map<User>(user);
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
    }
}
