using Khaoticen.CookBook.Api.Core.Exceptions.Base;

namespace Khaoticen.CookBook.Api.Core.Exceptions;

public class ValueNotAllowedException : DomainException
{
    public ValueNotAllowedException(string? message = null)
        : base(message ?? "Value not allowed.")
    {
    }
}