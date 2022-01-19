using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DTOs
{
    public class UserInputDTO
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [StringLength(400)]
        public string ImageUrl { get; set; }
        [Required]
        [StringLength(200)]
        public string Email { get; set; }
    }
}
