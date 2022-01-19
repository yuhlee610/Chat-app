using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class MessageUser
    {
        [StringLength(40)]
        public string MessageId { get; set; }
        public Message Message { get; set; }
        [StringLength(40)]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
