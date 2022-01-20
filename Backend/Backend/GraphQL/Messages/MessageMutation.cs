using Backend.DTOs;
using Backend.IRepository;
using Backend.Models;
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
        private readonly IMessageRepository _messageRepository;
        public MessageMutation(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<Message> CreateMessage(MessageInputDTO messageInput)
            => await _messageRepository.CreateMessage(messageInput);
    }
}
