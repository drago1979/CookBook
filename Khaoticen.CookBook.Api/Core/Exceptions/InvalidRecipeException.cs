using Khaoticen.CookBook.Api.Core.Exceptions.Base;

namespace Khaoticen.CookBook.Api.Core.Exceptions;

public class InvalidRecipeException: DomainException
{
    public InvalidRecipeException(string? message = null)
        : base(message ?? "Recipe must have at least one category.")
    {
    }
}