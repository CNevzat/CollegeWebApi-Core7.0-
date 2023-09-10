using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Validators
{
    public class DateCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
           var date = (DateTime?)value;

            if (date < DateTime.Now)
            {
                return new ValidationResult("The data must be greater than or equal to todays date");
            }
            return ValidationResult.Success;
        }
    }
}
