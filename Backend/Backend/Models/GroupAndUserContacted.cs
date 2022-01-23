using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class GroupAndUserContacted
    {
        public GroupAndUserContacted()
        {
            ContactedGroups = new List<GroupContacted>();
            ContactedUsers = new List<UserContacted>();
        }
        public List<UserContacted> ContactedUsers { get; set; }
        public List<GroupContacted> ContactedGroups { get; set; }
    }
}
