using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs
{
    public class AddGroupPlayload
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [StringLength(40)]
        public string HostId { get; set; }
        public IList<string> GroupUserIds { get; set; }
    }
}
