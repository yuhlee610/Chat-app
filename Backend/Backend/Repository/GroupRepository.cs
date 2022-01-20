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
        public async Task<Group> CreateGroup(GroupInputDTO groupInput)
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

                    return groupAdded;
                }
                catch(Exception ex)
                {
                    throw new GraphQLException(new Error("Server errors", "SERVER_ERRORS"));
                }
            }
        }
    }
}
