﻿using Polaby.Services.Models.NutrientModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.MenuModels
{
    public class MenuImportModel
    {
        [Required(ErrorMessage = "Menu name is required!")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Description is required!")]
        public string? Description { get; set; }
        public string? Image { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Protein must be a non-negative value.")]
        public float? Protein { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Carbohydrates must be a non-negative value.")]
        public float? Carbohydrates { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Fat must be a non-negative value.")]
        public float? Fat { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Alco must be a non-negative value.")]
        public float? Alco { get; set; }
        [Range(0, float.MaxValue, ErrorMessage = "Fiber must be a non-negative value.")]
        public float? Fiber { get; set; }
        public List<NutrientImportModel>? Nutrients { get; set; }
        public List<Guid> MealIds { get; set; }

    }
}
