using Khaoticen.CookBook.Api.Core.Exceptions.Base;

namespace Khaoticen.CookBook.Api.Core.Exceptions;

public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string? message = null)
        : base(message ?? "Entity not found.")
    {
    }

    public EntityNotFoundException(string entityName, object? key = null)
        : base(key is null
            ? $"Entity: '{entityName}' not found."
            : $"Entity: '{entityName}' with key '{key}' not found.")
    {
    }
}