using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class GroupContacted
    {
        public Group Group { get; set; }
        public Message LatestMessage { get; set; }
    }
}
