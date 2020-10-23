using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace RezaB.Web.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class PercentageAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value is int || value is long || value is short || string.IsNullOrEmpty(value as string))
                return ValidationResult.Success;
            var ds = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (Regex.IsMatch(
                value as string,
                @"^([1-9]{0,1}[0-9]{0,1}([" + ds + @"]\d{1,2}){0,1}|100([" + ds + @"][0]{1,2}){0,1})$",
                RegexOptions.ECMAScript))
                return ValidationResult.Success;
            return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName));
        }
    }
}