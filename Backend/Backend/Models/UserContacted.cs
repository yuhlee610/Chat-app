using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class UserContacted
    {
        public User User { get; set; }
        public Message LatestMessage { get; set; }
    }
}
