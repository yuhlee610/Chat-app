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
        public async Task<Message> CreateMessage(
            AddMessagePayload messageInput, ITopicEventSender eventSender)
        {
            using (ApplicationDbContext context = _contextFactory.CreateDbContext())
            {
                try
                {
                    Message messageAdd = new Message();
                    messageAdd = _mapper.Map<Message>(messageInput);
                    messageAdd.Date = DateTime.UtcNow;
                    messageAdd.IsLatest = true;

                    Message latestMessage = new Message();

                    if (Models.Type.ToGroup.CompareTo(messageInput.Type) == 0)
                    {
                        messageAdd.ToGroupId = messageInput.GroupOrUserId;
                        latestMessage = await context.Messages
                            .Where(m => (m.ToGroupId == messageInput.GroupOrUserId &&
                            m.IsLatest == true))
                            .FirstOrDefaultAsync();
                    }
                    else if (Models.Type.ToUser.CompareTo(messageInput.Type) == 0)
                    {
                        messageAdd.ToUserId = messageInput.GroupOrUserId;
                        latestMessage = await context.Messages
                            .Where(m => (m.SendByUserId == messageInput.SendByUserId
                            || m.SendByUserId == messageInput.GroupOrUserId) &&
                            (m.ToUserId == messageInput.SendByUserId ||
                            m.ToUserId == messageInput.GroupOrUserId) && m.IsLatest == true)
                            .FirstOrDefaultAsync();
                    }
                    if (latestMessage != null)
                    {
                        latestMessage.IsLatest = false;
                        context.Messages.Update(latestMessage);
                    }
                    await context.Messages.AddAsync(messageAdd);
                    await context.SaveChangesAsync();

                    if (Models.Type.ToUser.CompareTo(messageInput.Type) == 0)
                    {
                        await eventSender.SendAsync(
                            $"{messageAdd.SendByUserId}_{nameof(GraphQL.Messages.MessageSubscription.MessageCreated)}",
                            messageAdd);
                        await eventSender.SendAsync(
                            $"{messageAdd.ToUserId}_{nameof(GraphQL.Messages.MessageSubscription.MessageCreated)}",
                            messageAdd);
                    }
                    else if (Models.Type.ToGroup.CompareTo(messageInput.Type) == 0)
                    {
                        var membersOfGroup = await context.GroupUsers
                            .Where(gu => gu.GroupId == messageAdd.ToGroupId)
                            .Select(gu => gu.UserId).ToListAsync();
                        foreach (var userId in membersOfGroup)
                        {
                            await eventSender.SendAsync(
                            $"{userId}_{nameof(GraphQL.Messages.MessageSubscription.MessageCreated)}",
                            messageAdd);
                        }
                    }

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
