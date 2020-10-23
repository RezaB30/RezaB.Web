using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RezaB.Web.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TimeOfDayAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value is int || value is long || value is short)
                return ValidationResult.Success;

            if (Regex.IsMatch(
                value as string,
                @"^(([0-1][0-9])|([2][0-3])|[0-9])\:(([0-5][0-9])|[0-9])$",
                RegexOptions.ECMAScript))
                return ValidationResult.Success;
            return new ValidationResult(string.Format(this.ErrorMessageString, validationContext.DisplayName));
        }
    }
}
