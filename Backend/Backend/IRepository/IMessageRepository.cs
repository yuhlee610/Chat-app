using Backend.DTOs;
using Backend.Models;
using HotChocolate.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.IRepository
{
    public interface IMessageRepository
    {
        Task<Message> CreateMessage(AddMessagePayload messageInput, ITopicEventSender eventSender);
    }
}
