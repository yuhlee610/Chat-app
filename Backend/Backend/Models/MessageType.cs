using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class MessageType
    {
        [StringLength(40)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [StringLength(40)]
        public string Type { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
