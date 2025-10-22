using Khaoticen.CookBook.Api.Core.Exceptions.Base;

namespace Khaoticen.CookBook.Api.Core.Exceptions;

public class ValueNotAllowedException(string message) : DomainException(message);