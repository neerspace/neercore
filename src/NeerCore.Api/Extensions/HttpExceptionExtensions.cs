using FluentValidation;
using NeerCore.Exceptions;

namespace NeerCore.Api.Extensions;

public static class HttpExceptionExtensions
{
    public static Error CreateError(this HttpException e) => new(
        status: (int)e.StatusCode,
        type: e.ErrorType,
        message: e.Message,
        errors: e.Details
    );

    public static Error CreateFluentValidationError(this ValidationException e) => new(
        status: 400,
        type: "ValidationFailed",
        message: "Invalid model received.",
        errors: e.Errors.ToDictionary(ve => ve.PropertyName, ve => ve.ErrorMessage as object)
    );
}