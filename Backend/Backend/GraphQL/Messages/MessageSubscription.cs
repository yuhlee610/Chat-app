using Backend.Models;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL.Messages
{
    [ExtendObjectType(Name = "Subscription")]
    public class MessageSubscription
    {
        [SubscribeAndResolve]
        public ValueTask<ISourceStream<Message>> MessageCreated(
            string id, [Service] ITopicEventReceiver receiver)
        {
            var topic = $"{id}_{nameof(MessageCreated)}";
            return receiver.SubscribeAsync<string, Message>(topic);
        }
    }
}
