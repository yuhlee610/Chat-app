using Backend.DTOs;
using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.IRepository
{
    public interface IMessageRepository
    {
        Task<Message> CreateMessage(MessageInputDTO messageInput);
    }
}
