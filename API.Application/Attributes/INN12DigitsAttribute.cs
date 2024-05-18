using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace API.WebApi.Attributes
{
    public class INN12DigitsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            string inn = value.ToString();
            if (!inn.All(char.IsDigit))
            {
                return new ValidationResult("INN must be a number");
            }

            if (inn.Length != 12)
            {
                return new ValidationResult("INN must be a 12 digit number");
            }

            return ValidationResult.Success;
        }
    }
}
