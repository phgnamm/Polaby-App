﻿using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.AccountModels
{
    public class AccountLoginModel
    {
        [Required(ErrorMessage = "Email is required"), EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(256, ErrorMessage = "Email must be no more than 256 characters")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(128, MinimumLength = 8, ErrorMessage = "Password must be from 8 to 128 characters")]
        public required string Password { get; set; }
    }
}