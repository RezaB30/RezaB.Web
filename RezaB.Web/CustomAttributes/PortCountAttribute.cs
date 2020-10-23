using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RezaB.Web.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class PortCountAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value is int || value is long || value is short)
                return ValidationResult.Success;
            ushort parsed;
            if (ushort.TryParse((string)value, out parsed))
            {
                if(parsed < 50 || parsed > 5000)
                    return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName));
                return ValidationResult.Success;
            }
            return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName));
        }
    }
}