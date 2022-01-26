using Backend.DTOs;
using Backend.IRepository;
using Backend.Models;
using HotChocolate;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL.Messages
{
    [ExtendObjectType(Name = "Mutation")]
    public class MessageMutation
    {
        public async Task<Message> CreateMessage(
            AddMessagePayload messageInput, [Service] IMessageRepository messageRepository)
            => await messageRepository.CreateMessage(messageInput);
    }
}
