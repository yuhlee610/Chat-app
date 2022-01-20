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
    public class MessageRepository : IMessageRepository
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly IMapper _mapper;
        public MessageRepository(
            IDbContextFactory<ApplicationDbContext> contextFactory,
            IMapper mapper)
        {
            _mapper = mapper;
            _contextFactory = contextFactory;
        }
        public async Task<Message> CreateMessage(MessageInputDTO messageInput)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    Message messageAdd = new Message();
                    messageAdd = _mapper.Map<Message>(messageInput);
                    if (Models.Type.ToGroup.CompareTo(messageInput.Type) == 0)
                    {
                        messageAdd.ToGroupId = messageInput.GroupOrUserId;
                        
                    }
                    else if (Models.Type.ToUser.CompareTo(messageInput.Type) == 0)
                    {
                        messageAdd.ToUserId = messageInput.GroupOrUserId;
                    }
                    await context.Messages.AddAsync(messageAdd);
                    await context.SaveChangesAsync();
                    return messageAdd;
                }
                catch (Exception ex)
                {
                    throw new GraphQLException(new Error("Server errors", "SERVER_ERRORS"));
                }
            }            
        }
    }
}
