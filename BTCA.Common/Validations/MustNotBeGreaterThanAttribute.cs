using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BTCA.Common.Validations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MustNotBeGreaterThanAttribute : ValidationAttribute
    {
        private string _otherPropertyDisplayName;
        private readonly string _otherPropertyName;
        private readonly string _prefix;

        public MustNotBeGreaterThanAttribute(string otherPropName, string prefix = "") : 
            this(otherPropName, "{0} must not be greater than {1}", prefix)
        {            
        }

        public MustNotBeGreaterThanAttribute(string otherPropName, string errMsg, string prefix) : base(errMsg)
        {
            _otherPropertyName = otherPropName;
            _otherPropertyDisplayName = otherPropName;
            _prefix = prefix;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, _otherPropertyName);
        }

        internal void SetOtherPropertyName(PropertyInfo otherPropertyInfo)
        {
            var displayAttribute = otherPropertyInfo
                        .GetCustomAttributes<DisplayAttribute>()
                        .FirstOrDefault();

            _otherPropertyDisplayName = displayAttribute?.Name ?? _otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherPropertyName);
            SetOtherPropertyName(otherPropertyInfo);

            if (!int.TryParse(value.ToString(), out int toValidate))
            {
                return new ValidationResult($"{validationContext.DisplayName} must be numeric.");
            }

            var otherValue = (int)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
            return toValidate > otherValue ? new ValidationResult(FormatErrorMessage(validationContext.DisplayName)) : ValidationResult.Success;
        }
    }
}
