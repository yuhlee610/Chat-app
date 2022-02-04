using Backend.DTOs;
using Backend.Models;
using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL.Groups
{
    [ExtendObjectType(Name = "Subscription")]
    public class GroupSubscription
    {
        [SubscribeAndResolve]
        public ValueTask<ISourceStream<ContactGroup>> GroupCreated(
            string id, [Service] ITopicEventReceiver receiver)
        {
            var topic = $"{id}_{nameof(GroupCreated)}";
            return receiver.SubscribeAsync<string, ContactGroup>(topic);
        }

        [SubscribeAndResolve]
        public ValueTask<ISourceStream<ContactGroup>> GroupExited(
            string userId, [Service] ITopicEventReceiver receiver)
        {
            var topic = $"{userId}_{nameof(GroupExited)}";
            return receiver.SubscribeAsync<string, ContactGroup>(topic);
        }

        [SubscribeAndResolve]
        public ValueTask<ISourceStream<ContactGroup>> GroupAddMembers(
            string userId, [Service] ITopicEventReceiver receiver)
        {
            var topic = $"{userId}_{nameof(GroupAddMembers)}";
            return receiver.SubscribeAsync<string, ContactGroup>(topic);
        }

        [SubscribeAndResolve]
        public ValueTask<ISourceStream<ContactGroup>> GroupRemoveMembers(
            string userId, [Service] ITopicEventReceiver receiver)
        {
            var topic = $"{userId}_{nameof(GroupRemoveMembers)}";
            return receiver.SubscribeAsync<string, ContactGroup>(topic);
        }
    }
}
