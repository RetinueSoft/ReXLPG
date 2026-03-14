using FluentValidation;
using FluentValidation.Results;
using System;

namespace ReXLPgDAS.Util
{
    public static class ValidatorExtensions
    {
        public static void ValidateAndThrow<T>(this IValidator<T> validator, T entity)
        {
            ValidationResult result = validator.Validate(entity);

            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
        }
    }
}
