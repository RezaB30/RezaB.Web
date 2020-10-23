using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RezaB.Web.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CurrencyAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value is int || value is long || value is short)
                return ValidationResult.Success;

            var decimalSeperator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
            var groupSeperator = CultureInfo.CurrentCulture.NumberFormat.CurrencyGroupSeparator;

            if (Regex.IsMatch(
                value as string,
                @"^\d{1,3}(?:[" + groupSeperator + @"]?\d{3})*(?:[" + decimalSeperator + @"]\d{1,2})?$",
                RegexOptions.ECMAScript))
                return ValidationResult.Success;
            return new ValidationResult(string.Format(this.ErrorMessageString, validationContext.DisplayName));
        }
    }
}