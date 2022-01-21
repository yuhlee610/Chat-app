using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class GroupAndUserContacted
    {
        public List<User> ContactedUsers { get; set; }
        public List<Group> ContactedGroups { get; set; }
    }
}
