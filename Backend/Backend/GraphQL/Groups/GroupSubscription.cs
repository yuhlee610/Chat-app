﻿using Backend.DTOs;
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
    }
}
