using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class User
    {
        [StringLength(40)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [StringLength(400)]
        public string ImageUrl { get; set; }
        [Required]
        [StringLength(200)]
        public string Email { get; set; }
        public IList<Group> Groups { get; set; }
        public IList<GroupUser> GroupUsers { get; set; }
        public IList<Message> MessageToUser { get; set; }
        public IList<Message> MessagesOfUser { get; set; }
    }
}
