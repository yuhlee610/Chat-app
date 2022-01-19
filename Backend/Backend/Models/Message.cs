using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Message
    {
        [StringLength(40)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [StringLength(1000)]
        public string Content { get; set; }
        [StringLength(40)]
        public string SendBy { get; set; }
        [StringLength(40)]
        public string TypeId { get; set; }
        public MessageType MessageType { get; set; }
        public User User { get; set; }
        public IList<GroupMessage> GroupMessages { get; set; }
        public IList<MessageUser> MessageUsers { get; set; }

    }
}
