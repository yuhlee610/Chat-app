using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Group
    {
        [StringLength(40)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public string HostId { get; set; }
        public User Host { get; set; }
        public IList<GroupUser> GroupUsers { get; set; }
        public IList<Message> MessageToGroup { get; set; }
    }
}
