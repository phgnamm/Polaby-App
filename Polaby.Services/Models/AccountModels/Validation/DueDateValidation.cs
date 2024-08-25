using System.ComponentModel.DataAnnotations;

namespace Polaby.Services.Models.AccountModels.Validation;

public class DueDateValidation : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return true;
        }
        
        if (value is not DateOnly dueDate)
        {
            return false;
        }

        var today = DateOnly.FromDateTime(DateTime.Now);
        var maxDueDate = today.AddDays(42 * 7); // 42 weeks * 7 days per week

        // return dueDate >= maxDueDate || dueDate <= today;
        return dueDate < maxDueDate && dueDate > today;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"The due date cannot be more than 42 weeks from today";
    }
}