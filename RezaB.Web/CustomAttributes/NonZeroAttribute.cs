using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class NonZeroAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value is int || value is long || value is short)
                return ValidationResult.Success;
            decimal parsed;
            if (decimal.TryParse(value as string, out parsed))
            {
                if(parsed == 0m)
                    return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
