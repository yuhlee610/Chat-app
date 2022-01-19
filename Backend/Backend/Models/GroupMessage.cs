using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class GroupMessage
    {
        [StringLength(40)]
        public string GroupId { get; set; }
        public Group Group { get; set; }
        [StringLength(40)]
        public string MessageId { get; set; }
        public Message Message { get; set; }
    }
}
