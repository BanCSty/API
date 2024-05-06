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
                return new ValidationResult("INN должен быть числом.");
            }

            string innString = inn.ToString();
            if (innString.Length != 12)
            {
                return new ValidationResult("INN должен состоять из 12 цифр.");
            }

            return ValidationResult.Success;
        }
    }
}
