using Backend.DTOs;
using Backend.IRepository;
using Backend.Models;
using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL.Messages
{
    [ExtendObjectType(Name = "Mutation")]
    [Authorize]
    public class MessageMutation
    {
        public async Task<Message> CreateMessage(
            AddMessagePayload messageInput, [Service] IMessageRepository messageRepository,
            [Service] ITopicEventSender eventSender)
            => await messageRepository.CreateMessage(messageInput, eventSender);
    }
}
