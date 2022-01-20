using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;

namespace Backend.DTOs
{
    public class MessageInputDTO
    {
        [Required]
        [StringLength(1000)]
        public string Content { get; set; }
        [Required]
        [StringLength(40)]
        public string SendByUserId { get; set; }
        [Required]
        public Models.Type Type { get; set; }
        public string GroupOrUserId { get; set; }
    }
}
