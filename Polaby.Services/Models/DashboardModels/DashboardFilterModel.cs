﻿using System.ComponentModel.DataAnnotations;
using Polaby.Services.Models.DashboardModels.Validation;

namespace Polaby.Services.Models.DashboardModels;

public class DashboardFilterModel
{
    private static readonly int CurrentMonth = DateTime.Today.Month;
    private static readonly int CurrentYear = DateTime.Today.Year;

    [Range(1, 12, ErrorMessage = "AmountMonth must be between 1 and 12.")]
    public int Month { get; set; } = CurrentMonth;

    [CustomYearValidation(ErrorMessage = "CommentYear cannot be greater than the current year.")]
    public int Year { get; set; } = CurrentYear;
}