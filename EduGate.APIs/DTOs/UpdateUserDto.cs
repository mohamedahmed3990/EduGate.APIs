﻿using System.ComponentModel.DataAnnotations;

namespace EduGate.APIs.DTOs
{
    public class UpdateUserDto
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
