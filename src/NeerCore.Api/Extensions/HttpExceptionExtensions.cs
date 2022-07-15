using FluentValidation;
using NeerCore.Exceptions;

namespace NeerCore.Api.Extensions;

public static class HttpExceptionExtensions
{
    public static Error CreateError(this HttpException e) => new(
        status: (int) e.StatusCode,
        type: e.ErrorType,
        message: e.Message,
        errors: e.Details?.Select(ed => new Error.Details(ed.Field, ed.Message)).ToArray()
    );

    public static Error CreateFluentValidationError(this ValidationException e) => new(
        status: 400,
        type: "ValidationFailed",
        message: "Invalid model received.",
        errors: e.Errors.Select(ve => new Error.Details(ve.PropertyName, ve.ErrorMessage)).ToArray()
    );
}