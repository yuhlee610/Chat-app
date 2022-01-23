using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public enum Type
    {
        ToUser,
        ToGroup
    }
    public class Message
    {
        [StringLength(40)]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [StringLength(1000)]
        public string Content { get; set; }
        [StringLength(40)]
        public string SendByUserId { get; set; }
        [Required]
        public Type Type { get; set; }
        public User SendByUser { get; set; }
        [StringLength(40)]
        public string ToUserId { get; set; }
        public User ToUser { get; set; }
        [StringLength(40)]
        public string ToGroupId { get; set; }
        public Group ToGroup { get; set; }
        public DateTime Date { get; set; }
        public bool IsLatest { get; set; }
    }
}
