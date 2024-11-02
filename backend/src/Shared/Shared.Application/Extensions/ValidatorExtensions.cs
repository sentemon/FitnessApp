using FluentValidation;

namespace Shared.Application.Extensions;

public static class ValidatorExtensions
{
    public static async Task<string?> ValidateResultAsync<T>(this IValidator<T> validator, T instance)
    {
        var result = await validator.ValidateAsync(instance);
        
        if (!result.IsValid)
        {
            return string.Join("; ", result.Errors.Select(e => e.ErrorMessage));
        }

        return null;
    }
}