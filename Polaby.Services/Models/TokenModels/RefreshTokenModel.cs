﻿using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.TokenModels
{
    public class RefreshTokenModel
    {
        [Required(ErrorMessage = "Access token is required")]
        public required string AccessToken { get; set; }

        public string? RefreshToken { get; set; }
    }
}