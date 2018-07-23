using System;
using System.ComponentModel.DataAnnotations;

namespace BTCA.Common.Validations
{
    public class NotNegativeAttribute : ValidationAttribute
    {
        public NotNegativeAttribute() : this("{0} must not be negative.") 
        {            
        }    

        public NotNegativeAttribute(string errorMessage) : base(errorMessage)
        {            
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name);
        } 

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!int.TryParse(value.ToString(), out int result))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            if (result >= 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }                   
    }
}
