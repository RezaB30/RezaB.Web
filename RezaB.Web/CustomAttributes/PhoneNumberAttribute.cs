﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RezaB.Web.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class PhoneNumber : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value is int || value is long || value is short)
                return ValidationResult.Success;
            if (Regex.IsMatch(value as string, @"^[1-9]{1}[0-9]{9}$", RegexOptions.ECMAScript))
                return ValidationResult.Success;
            return new ValidationResult(string.Format(ErrorMessageString, validationContext.DisplayName));
        }
    }
}