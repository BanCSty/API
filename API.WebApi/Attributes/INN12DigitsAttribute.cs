using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            long inn;
            if (!long.TryParse(value.ToString(), out inn))
            {
                return new ValidationResult("INN must be a number");
            }

            string innString = inn.ToString();
            if (innString.Length != 12)
            {
                return new ValidationResult("INN must be a 12 digit number");
            }

            return ValidationResult.Success;
        }
    }
}
