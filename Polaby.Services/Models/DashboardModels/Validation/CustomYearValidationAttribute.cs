﻿using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.DashboardModels.Validation;

public class CustomYearValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var year = (int)value;

        if (year > DateTime.Today.Year)
        {
            return new ValidationResult(ErrorMessage ?? "Year cannot be greater than the current year.");
        }

        return ValidationResult.Success;
    }
}