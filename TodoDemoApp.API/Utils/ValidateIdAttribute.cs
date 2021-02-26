using System;
using System.ComponentModel.DataAnnotations;

namespace TodoDemoApp.API.Utils
{
    public class ValidateIdAttribute : ValidationAttribute
    {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var idValue = (Guid) value;

            if (idValue == Guid.Empty)
            {
                return new ValidationResult(FormatErrorMessage("Lütfen geçerli bir kategori giriniz."));
            }
            else return ValidationResult.Success;
        }

    }
}
