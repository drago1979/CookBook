using Khaoticen.CookBook.Api.Core.Exceptions.Base;

namespace Khaoticen.CookBook.Api.Core.Exceptions;

public class InvalidRecipeException(string message) : DomainException(message);