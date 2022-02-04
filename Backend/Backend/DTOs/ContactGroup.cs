using Backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs
{
    public class ContactGroup
    {
        public Group Group { get; set; }
        public int numOfMembers { get; set; }
        public Message? LatestMessage { get; set; }
    }
}
