using Backend.Data;
using Backend.Models;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.GraphQL.Messages
{
    [ExtendObjectType(Name = "Query")]
    [Authorize]
    public class MessageQuery
    {
        [UseDbContext(typeof(ApplicationDbContext))]
        [UseSorting]
        [UseFiltering]
        public IQueryable<Message> GetMessages(
            [ScopedService] ApplicationDbContext context)
        {
            IQueryable<Message> query = context.Messages;
            return query.Include("SendByUser").Include("ToUser").Include("ToGroup");
        }
    }
}
